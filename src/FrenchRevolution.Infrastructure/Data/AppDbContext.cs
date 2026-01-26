using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<CharacterOffice> CharacterOffices => Set<CharacterOffice>();
    public DbSet<Faction> Factions => Set<Faction>();
    public DbSet<CharacterFaction> CharacterFactions => Set<CharacterFaction>();
    
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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