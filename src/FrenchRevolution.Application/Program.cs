using System.Threading.RateLimiting;
using FluentValidation;
using FrenchRevolution.Application.Constants;
using FrenchRevolution.Application.Exceptions;
using FrenchRevolution.Application.Validation;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

// MediatR
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
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.MapControllers().RequireRateLimiting(RateLimiting.FixedWindow);

app.UseAuthorization();

app.UseRateLimiter();

app.Run();