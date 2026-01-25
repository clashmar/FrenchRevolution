using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace FrenchRevolution.IntegrationTests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = 
        new PostgreSqlBuilder("postgres:15.1-alpine")
            .WithDatabase("french_revolution_test")
            .WithUsername("test")
            .WithPassword("test")
            .WithCleanUp(true)
            .WithReuse(false)
            .Build();

    private string ConnectionString => _container.GetConnectionString();
    private Respawner _respawner = null!;
    private readonly SemaphoreSlim _contextLock = new(1, 1);
    
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await using var context = CreateDbContext();

        await context.Database.MigrateAsync();
        
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            WithReseed = true
        });
    }

    public async Task DisposeAsync()
    {
        _contextLock.Dispose();
        await _container.DisposeAsync();
    }
    
    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .EnableSensitiveDataLogging(false)     
            .UseNpgsql(
                ConnectionString,
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly)
            )
            .Options;
        
        var context = new AppDbContext(options);
        return context;
    }

    public async Task ResetDatabaseAsync()
    {
        await _contextLock.WaitAsync();
        try
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            await _respawner.ResetAsync(connection);
        }
        finally
        {
            _contextLock.Release();
        }
    }
}