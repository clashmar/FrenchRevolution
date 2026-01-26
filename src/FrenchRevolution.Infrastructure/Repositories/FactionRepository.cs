using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class FactionRepository(AppDbContext context) : IFactionRepository
{
    public async Task<Faction?> GetByTitleAsync(string title, CancellationToken ct = default)
    {
        var normalizedTitle = title.Trim().ToLowerInvariant();
        return await context.Factions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                f => f.NormalizedTitle == normalizedTitle,
                ct
                );
    }

    public async Task<Faction?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Factions
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IEnumerable<Faction>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Factions
            .AsNoTracking()
            .OrderBy(f => f.Title)
            .ToListAsync(ct);
    }

    public Guid Add(Faction faction)
    {
        context.Factions.Add(faction);
        return faction.Id;
    }
}
