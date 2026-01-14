using FrenchRevolution.Infrastructure.Data;
using FrenchRevolution.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FrenchRevolution.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = 
        new PostgreSqlBuilder("postgres:15.1")
            .WithDatabase("french_revolution_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

    private string ConnectionString => _container.GetConnectionString();
    
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
    
    public async Task<TestAppDbContext> CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        var context = new TestAppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        
        return context;
    }

    public async Task ResetDatabaseAsync()
    {
        var context = await CreateDbContextAsync();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await context.DisposeAsync();
    }
}