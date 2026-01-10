using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;

namespace FrenchRevolution.Domain.Services;

public interface ICharacterService
{
    Task<IEnumerable<Character>> GetAllAsync();
    Task<Character?> GetByIdAsync(Guid id);
    Task<Character?> CreateAsync(Character character);
    Task<Character?> UpdateAsync(Guid id, Character request);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterService(ICharacterRepository repository) : ICharacterService
{
    public async Task<IEnumerable<Character>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public Task<Character?> GetByIdAsync(Guid id)
    {
        return repository.GetByIdAsync(id);
    }

    public Task<Character?> CreateAsync(Character character)
    {
        return repository.AddAsync(character);
    }

    public async Task<Character?> UpdateAsync(Guid id, Character request)
    {
        var character = await repository.GetByIdAsync(id);
        
        if (character is null)
        {
            return null;
        }
        
        character.Update(
            request.Name,
            request.Profession,
            request.DateOfBirth,
            request.DateOfDeath
            );
        
        return await repository.UpdateAsync(character);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var character = await repository.GetByIdAsync(id);
        
        if (character is null)
        {
            return false;
        }
        
        return await repository.DeleteAsync(character);
    }
}