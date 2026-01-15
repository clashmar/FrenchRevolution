using System.Linq.Expressions;
using FrenchRevolution.Domain.Constants;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Repositories;

public class CharacterRepository(
    AppDbContext context
    ) : ICharacterRepository
{
    public async Task<(IEnumerable<Character> Items, int TotalCount)> GetAllAsync(
        string? nameFilter = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default
        )
    {
        var query = context.Characters
            .Include(c => c.CharacterRoles)
            .ThenInclude(rc => rc.Role)
            .AsQueryable(); 

        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            var pattern = $"%{nameFilter.Trim()}%";
            query = query.Where(c => EF.Functions.ILike(c.Name, pattern));
        }
        
        var totalCount = await query.CountAsync(ct);
        
        var sortProperty = GetSortProperty(sortColumn);
        query = sortOrder?.ToLower() == QueryValues.Desc
            ? query.OrderByDescending(sortProperty) 
            : query.OrderBy(sortProperty);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return (items, totalCount);
    }

    public async Task<Character?> GetByIdAsync(Guid id, CancellationToken ct  = default) 
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
    
    private static Expression<Func<Character, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            QueryValues.Born => c => c.DateOfBirth,
            QueryValues.Died => c => c.DateOfDeath,
            _ => c => c.Name
        };
    }
}