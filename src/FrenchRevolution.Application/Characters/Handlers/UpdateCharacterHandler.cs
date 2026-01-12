using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class UpdateCharacterHandler(
    ICharacterRepository repository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateCharacterCommand, bool>
{
    public async Task<bool> Handle(
        UpdateCharacterCommand command,
        CancellationToken cancellationToken)
    {
        var character = await repository.GetByIdAsync(command.Id);
        
        if (character is null)
        {
            return false;
        }
        
        character.Update(
            command.Character.Name,
            command.Character.Profession,
            command.Character.DateOfBirth,
            command.Character.DateOfDeath
        );
        
        repository.Update(character);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}