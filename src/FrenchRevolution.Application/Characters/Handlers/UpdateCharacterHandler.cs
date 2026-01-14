using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class UpdateCharacterHandler(
    ICharacterRepository characterRepository,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository
) : IRequestHandler<UpdateCharacterCommand, bool>
{
    public async Task<bool> Handle(
        UpdateCharacterCommand command,
        CancellationToken ct)
    {
        var character = await characterRepository.GetByIdAsync(command.Id, ct);
        
        if (character is null)
        {
            return false;
        }
        
        // Update basic properties
        character.Update(
            command.Request.Name,
            command.Request.Profession,
            command.Request.DateOfBirth,
            command.Request.DateOfDeath
        );
        
        // Clear existing roles
        character.ClearRoles();
        
        // Assign new roles
        foreach (var roleDto in command.Request.Roles)
        {
            var role = await roleRepository.GetByTitleAsync(roleDto.Title, ct);

            if (role is null)
            {
                role = new Role(roleDto.Title);
                roleRepository.Add(role);
            }

            character.AssignRole(role, roleDto.From, roleDto.To);
        }
        
        characterRepository.Update(character);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}