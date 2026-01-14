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
        CancellationToken ct)
    {
        var character = await repository.GetByIdAsync(command.Id, ct);

        if (character is null)
        {
            return false;
        }
        
        repository.Remove(character);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}