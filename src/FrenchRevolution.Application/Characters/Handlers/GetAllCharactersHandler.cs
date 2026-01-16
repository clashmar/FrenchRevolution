using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Constants;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

using static PagedList<CharacterResponseDto>;

public class GetAllCharactersHandler(
    ICacheAside cacheAside,
    ICharacterRepository repository
) : IRequestHandler<GetAllCharactersQuery, PagedList<CharacterResponseDto>>
{
    private const string BaseKey = "characters:all"; 
    
    public async Task<PagedList<CharacterResponseDto>> Handle(
        GetAllCharactersQuery query,
        CancellationToken ct)
    {
        var cacheKey = ConstructCacheKey(query);
        
        return await cacheAside.GetOrCreateAsync(
            cacheKey, 
            async token => 
            { 
                var (characters, totalCount) = await repository.GetAllAsync(
                        query.Name, 
                        query.SortColumn,
                        query.SortOrder,
                        query.Page,
                        query.PageSize,
                        token);
                
                var dtos = characters
                    .Select(c => c.ToResponseDto())
                    .ToList();
                
                return CreatePagedListAsync(
                    dtos,
                    query.Page,
                    query.PageSize,
                    totalCount
                    );
            }, 
            ct: ct) ?? EmptyPagedList();
    }

    private static string ConstructCacheKey(GetAllCharactersQuery query)
    {
        var namekey = (query.Name ?? QueryValues.All).Trim().ToLowerInvariant();
        var colKey = (query.SortColumn ?? QueryValues.Name).Trim().ToLowerInvariant();
        var orderKey = (query.SortOrder ?? QueryValues.Asc).Trim().ToLowerInvariant();

        return $"{BaseKey}:{namekey}:{colKey}:{orderKey}:{query.Page}:{query.PageSize}";
    }
}