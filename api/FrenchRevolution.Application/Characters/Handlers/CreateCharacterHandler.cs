using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class CreateCharacterHandler(
    ICharacterRepository repository
    ) : IRequestHandler<CreateCharacterCommand, Character?>
{
    async Task<Character?> IRequestHandler<CreateCharacterCommand, Character?>
        .Handle(
            CreateCharacterCommand command,
            CancellationToken cancellationToken)
    {
        return await repository.AddAsync(command.Character);
    }
}