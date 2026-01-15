using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Domain.Repositories;

public interface ICharacterRepository
{
    Task<(IEnumerable<Character> Items, int TotalCount)> GetAllAsync(
        string? name = null, 
        string? sortColumn = null,
        string? sortOrder = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default
        );
    Task<Character?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Guid Add(Character character);
    void Update(Character character);
    void Remove(Character character);
}