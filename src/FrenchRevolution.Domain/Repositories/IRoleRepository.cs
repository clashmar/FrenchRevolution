using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByTitleAsync(string title, CancellationToken ct = default);
    Guid Add(Role role);
}