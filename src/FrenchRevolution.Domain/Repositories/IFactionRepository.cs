using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Domain.Repositories;

public interface IFactionRepository
{
    Task<Faction?> GetByTitleAsync(string title, CancellationToken ct = default);
    Task<Faction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Faction>> GetAllAsync(CancellationToken ct = default);
    Guid Add(Faction faction);
}
