using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class CharacterRepository(
    AppDbContext context
    ) : ICharacterRepository
{
    public async Task<IEnumerable<Character>> GetAllAsync()
    {
        return await context.Characters.ToListAsync();
    }

    public async Task<Character?> GetByIdAsync(Guid id) 
    {
        return await context.Characters.FindAsync(id); 
    }

    public Guid Add(Character character)
    {
        context.Characters.Add(character);
        return character.Id;
    }

    public void Update(Character character)
    {
        context.Characters.Update(character);
    }

    public void Remove(Character character)
    {
        context.Characters.Remove(character);
    }
}