using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class CharacterRepository(
    AppDbContext context
    ) : ICharacterRepository
{
    public async Task<IEnumerable<Character>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Characters
            .Include(c => c.CharacterRoles)
                .ThenInclude(rc => rc.Role)
            .ToListAsync(ct);
    }

    public async Task<Character?> GetByIdAsync(Guid id, CancellationToken ct = default) 
    {
        return await context.Characters
            .Include(c => c.CharacterRoles)
                .ThenInclude(cr => cr.Role)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
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