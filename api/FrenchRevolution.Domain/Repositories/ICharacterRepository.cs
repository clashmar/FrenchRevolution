using FrenchRevolution.Domain.Entities;

namespace FrenchRevolution.Domain.Repositories;

public interface ICharacterRepository
{
    Task<IEnumerable<Character>> GetAllAsync();
    Task<Character?> GetByIdAsync(Guid id);
    Task<Character?> AddAsync(Character character);
    Task<Character?> UpdateAsync(Character character);
    Task<bool> DeleteAsync(Character character);
}