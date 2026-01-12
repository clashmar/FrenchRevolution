using FrenchRevolution.Domain.Entities;

namespace FrenchRevolution.Domain.Repositories;

public interface ICharacterRepository
{
    Task<IEnumerable<Character>> GetAllAsync();
    Task<Character?> GetByIdAsync(Guid id);
    Guid Add(Character character);
    void Update(Character character);
    void Delete(Character character);
}