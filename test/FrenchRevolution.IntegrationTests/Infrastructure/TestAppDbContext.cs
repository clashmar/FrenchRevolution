using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.IntegrationTests.Infrastructure;

public class TestAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Don't call base to skip seeding
    }
}