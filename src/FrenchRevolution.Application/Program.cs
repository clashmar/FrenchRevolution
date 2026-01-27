using System.Text;
using System.Threading.RateLimiting;
using Asp.Versioning;
using FluentValidation;
using FrenchRevolution.Application.Auth.Services;
using FrenchRevolution.Application.Behaviours;
using FrenchRevolution.Application.Config;
using FrenchRevolution.Application.Constants;
using FrenchRevolution.Application.Exceptions;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(context.Configuration));

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));

// Controllers & Behaviour
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(
            new ProblemDetails
            {
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
            });
    });

// Razor Pages for Admin Panel
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
    options.Conventions.AllowAnonymousToPage("/Admin/Login");
    options.Conventions.AllowAnonymousToPage("/Admin/AccessDenied");
});

builder.Services.AddProblemDetails(cfg =>
{
    cfg.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// API Documentation
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Rate Limiting
builder.Services.AddRateLimiter(rateLimitOptions =>
{
    rateLimitOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimitOptions.AddPolicy(RateLimiting.FixedWindow, context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? RateLimiting.Unknown,
        factory: _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromSeconds(10),
            PermitLimit = 3,
            QueueLimit = 3,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        }
    ));
});

// Validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"),
        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly)
    );
});

// Authentication
builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services
    .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtConfig?.Issuer,
            ValidAudience = jwtConfig?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SecretKey!)),
        };
    })
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminCookie");
        policy.RequireAuthenticatedUser();
        policy.RequireRole(Roles.Admin);
    });
});

// Mediator
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.LicenseKey = builder.Configuration["Mediator:LicenseKey"];
});

// Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
    options.InstanceName = "FrenchRevolution";
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("Database")!,
        name: "database",
        tags: ["db", "sql", "postgres"])
    .AddRedis(
        builder.Configuration.GetConnectionString("Cache")!,
        name: "redis-cache",
        tags: ["cache", "redis"]);

// OTEL
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("FrenchRevolution"))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddRedisInstrumentation();
    });

// Services
builder.Services.AddSingleton<ICacheAside, CacheAside>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IFactionRepository, FactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // TODO: extract to extension method
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    await SeedIdentity.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting(RateLimiting.FixedWindow);

app.MapRazorPages();

app.UseRateLimiter();

app.Run();
