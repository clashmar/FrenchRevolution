using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

public class UpdateCharacterHandler(
    ICharacterRepository characterRepository,
    IUnitOfWork unitOfWork,
    IOfficeRepository officeRepository,
    IFactionRepository factionRepository
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

        var existingCharacter = await characterRepository.GetByNameAsync(command.Request.Name, ct);
        if (existingCharacter is not null && existingCharacter.Id != character.Id)
        {
            throw new DuplicateCharacterException(command.Request.Name);
        }

        // Update basic properties
        character.Update(
            command.Request.Name,
            command.Request.Profession,
            command.Request.Born,
            command.Request.Died,
            new Portrait(command.Request.PortraitUrl)
        );

        // Clear existing offices
        character.ClearOffices();

        // Assign new offices
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

        // Clear existing factions
        character.ClearFactions();

        // Assign new factions
        foreach (var factionDto in command.Request.Factions)
        {
            var faction = await factionRepository.GetByTitleAsync(factionDto.Title, ct);

            if (faction is null)
            {
                faction = new Faction(factionDto.Title, string.Empty);
                factionRepository.Add(faction);
            }

            character.AssignFaction(faction);
        }

        characterRepository.Update(character);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}