using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class GetCharacterByIdHandler(
    ICharacterRepository repository
) : IRequestHandler<GetCharacterByIdQuery, Character?>
{
    async Task<Character?> IRequestHandler<GetCharacterByIdQuery, Character?>
        .Handle(
            GetCharacterByIdQuery command,
            CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(command.Id);
    }
}