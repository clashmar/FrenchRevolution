using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                SeedData.SeedStaticData(this);
                context.SaveChanges();

            }).UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await SeedData.SeedStaticDataAsync(this, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            });
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
        
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}