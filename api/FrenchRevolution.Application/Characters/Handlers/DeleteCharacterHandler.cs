using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class DeleteCharacterHandler(
    ICharacterRepository repository
) : IRequestHandler<DeleteCharacterCommand, bool>
{
    async Task<bool> IRequestHandler<DeleteCharacterCommand, bool>
        .Handle(
            DeleteCharacterCommand command,
            CancellationToken cancellationToken)
    {
        var character = await repository.GetByIdAsync(command.Id);

        if (character is null)
        {
            return false;
        }
        
        return await repository.DeleteAsync(character);
    }
}