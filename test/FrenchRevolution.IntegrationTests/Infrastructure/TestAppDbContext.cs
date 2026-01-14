using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.IntegrationTests.Infrastructure;

// TODO: Remove duplication with AppDbContext
public class TestAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Don't call base to skip seeding
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var softDeleteEntries = ChangeTracker.Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entityEntry in softDeleteEntries)
        {
            entityEntry.State = EntityState.Modified; 
            entityEntry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            entityEntry.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}