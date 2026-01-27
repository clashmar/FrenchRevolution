using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Constants;
using FrenchRevolution.Domain.Repositories;
using FrenchRevolution.Infrastructure.Cache;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetCharacterByIdHandler(
    ICacheAside cacheAside,
    ICharacterRepository repository
) : IRequestHandler<GetCharacterByIdQuery, CharacterResponseDto?>
{
    private const string BaseKey = "characters:id"; 
    
    public async Task<CharacterResponseDto?> Handle(
            GetCharacterByIdQuery command,
            CancellationToken ct)
    {
        return await cacheAside.GetOrCreateAsync(
            $"{BaseKey}:{command.Id}", 
            async token => 
            { 
                var character = await repository.GetByIdAsync(command.Id, token);
                return character?.ToResponseDto();
            }, 
            ct: ct);
    }
}