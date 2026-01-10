using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrenchRevolution.Infrastructure.Repositories;

public class CharacterRepository(
    AppDbContext context,
    ILogger<CharacterRepository> logger
    ) : ICharacterRepository
{
    public async Task<IEnumerable<Character>> GetAllAsync()
    {
        try
        {
            return await context.Characters.ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error getting all characters.");
            return [];
        }
    }

    public async Task<Character?> GetByIdAsync(Guid id)
    {
        try
        {
            return await context.Characters.FindAsync(id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error finding character by id.");
            return null;
        }
    }

    public async Task<Character?> AddAsync(Character character)
    {
        try
        {
            var result = await context.Characters.AddAsync(character); 
            await context.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error adding character.");
            return null;
        }
    }

    public async Task<Character?> UpdateAsync(Character character)
    {
        try
        {
            context.Characters.Update(character);
            await context.SaveChangesAsync();
            return character;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error updating character.");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Character character)
    {
        try
        {
            context.Characters.Remove(character);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error deleting character.");
            return false;
        }
    }
}