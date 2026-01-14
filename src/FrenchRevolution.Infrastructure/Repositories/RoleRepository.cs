using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByTitleAsync(string title,  CancellationToken ct = default)
    {
        var normalizedTitle = title.Trim().ToLowerInvariant();
        return await context.Roles.FirstOrDefaultAsync(
            r => r.NormalizedTitle == normalizedTitle, 
            ct);
    }

    public Guid Add(Role role)
    {
        context.Roles.Add(role);
        return role.Id;
    }
}