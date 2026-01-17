using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using FrenchRevolution.Application.Auth.Services;
using FrenchRevolution.Application.Config;
using FrenchRevolution.Application.Constants;
using FrenchRevolution.Application.Exceptions;
using FrenchRevolution.Application.Validation;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories; 
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args); 

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

// Problem details
builder.Services.AddProblemDetails(cfg =>
{ 
    cfg.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

builder.Logging.AddConsole();

builder.Logging.AddDebug();

// Rate limiting
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

// Db context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("FrenchRevolution.Infrastructure")
    );
});

// Identity
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

// Jwt Authentication
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
            ValidAudience =  jwtConfig?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SecretKey!)),
        };
    });

builder.Services.AddAuthorization();

// Mediator
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.LicenseKey = builder.Configuration["Mediator:LicenseKey"];
});

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "FrenchRevolution";
});

// Health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "database",
        tags: ["db", "sql", "postgres"])
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis")!,
        name: "redis-cache",
        tags: ["cache", "redis"]);

// Services
builder.Services.AddSingleton<ICacheAside, CacheAside>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    
    await SeedIdentity.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.UseExceptionHandler();

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.MapControllers().RequireRateLimiting(RateLimiting.FixedWindow);

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.Run();