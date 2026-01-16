using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Domain.Repositories;

public interface IOfficeRepository
{
    Task<Office?> GetByTitleAsync(string title, CancellationToken ct = default);
    Guid Add(Office office);
}