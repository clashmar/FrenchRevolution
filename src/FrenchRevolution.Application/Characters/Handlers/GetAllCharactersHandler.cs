using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetAllCharactersHandler(
    ICharacterRepository repository
) : IRequestHandler<GetAllCharactersQuery, IEnumerable<CharacterResponseDto>>
{
    public async Task<IEnumerable<CharacterResponseDto>> Handle(
        GetAllCharactersQuery query,
        CancellationToken ct)
    {
        return (await repository.GetAllAsync(ct))
            .Select(c => c.ToResponseDto());
    }
}