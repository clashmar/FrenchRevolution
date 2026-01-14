using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetCharacterByIdHandler(
    ICharacterRepository repository
) : IRequestHandler<GetCharacterByIdQuery, CharacterResponseDto?>
{
    public async Task<CharacterResponseDto?> Handle(
            GetCharacterByIdQuery command,
            CancellationToken ct)
    {
        var character = await repository.GetByIdAsync(command.Id, ct);
        return character?.ToResponseDto();
    }
}