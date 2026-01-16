using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class OfficeRepository(AppDbContext context) : IOfficeRepository
{
    public async Task<Office?> GetByTitleAsync(string title,  CancellationToken ct = default)
    {
        var normalizedTitle = title.Trim().ToLowerInvariant();
        return await context.Offices.FirstOrDefaultAsync(
            r => r.NormalizedTitle == normalizedTitle, 
            ct);
    }

    public Guid Add(Office office)
    {
        context.Offices.Add(office);
        return office.Id;
    }
}