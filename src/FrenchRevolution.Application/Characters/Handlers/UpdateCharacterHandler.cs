using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class UpdateCharacterHandler(
    ICharacterRepository characterRepository,
    IUnitOfWork unitOfWork,
    IOfficeRepository officeRepository
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
            command.Request.Born,
            command.Request.Died
        );
        
        // Clear existing roles
        character.ClearOffices();
        
        // Assign new roles
        foreach (var roleDto in command.Request.Offices)
        {
            var role = await officeRepository.GetByTitleAsync(roleDto.Title, ct);

            if (role is null)
            {
                role = new Office(roleDto.Title);
                officeRepository.Add(role);
            }

            character.AssignOffice(role, roleDto.From, roleDto.To);
        }
        
        characterRepository.Update(character);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}