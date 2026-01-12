using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Entities;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class UpdateCharacterHandler(
    ICharacterRepository repository
) : IRequestHandler<UpdateCharacterCommand, Character?>
{
    async Task<Character?> IRequestHandler<UpdateCharacterCommand, Character?>
        .Handle(
            UpdateCharacterCommand command,
            CancellationToken cancellationToken)
    {
        var character = await repository.GetByIdAsync(command.Id);
        
        if (character is null)
        {
            return null;
        }
        
        character.Update(
            command.Character.Name,
            command.Character.Profession,
            command.Character.DateOfBirth,
            command.Character.DateOfDeath
        );
        
        return await repository.UpdateAsync(character);
    }
}