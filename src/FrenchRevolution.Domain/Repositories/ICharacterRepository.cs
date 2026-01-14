using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Domain.Repositories;

public interface ICharacterRepository
{
    Task<IEnumerable<Character>> GetAllAsync(CancellationToken ct = default);
    Task<Character?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Guid Add(Character character);
    void Update(Character character);
    void Remove(Character character);
}