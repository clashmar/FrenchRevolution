using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class DeleteCharacterHandler(
    ICharacterRepository repository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteCharacterCommand, bool>
{
    public async Task<bool> Handle(
        DeleteCharacterCommand command,
        CancellationToken cancellationToken)
    {
        var character = await repository.GetByIdAsync(command.Id);

        if (character is null)
        {
            return false;
        }
        
        repository.Delete(character);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}