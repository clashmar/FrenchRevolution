using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetAllCharactersHandler(
    ICacheAside cacheAside,
    ICharacterRepository repository
) : IRequestHandler<GetAllCharactersQuery, IReadOnlyList<CharacterResponseDto>>
{
    private const string Key = "characters:all"; 
    
    public async Task<IReadOnlyList<CharacterResponseDto>> Handle(
        GetAllCharactersQuery query,
        CancellationToken ct)
    {
        return await cacheAside.GetOrCreateAsync(
            Key, 
            async token => 
            { 
                return (await repository.GetAllAsync(token))
                    .Select(c => c.ToResponseDto())
                    .ToList(); 
            }, 
            ct: ct) ?? [];
    }
}