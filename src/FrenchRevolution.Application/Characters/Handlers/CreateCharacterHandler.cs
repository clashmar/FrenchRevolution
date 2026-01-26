using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

internal sealed class CreateCharacterHandler(
    ICharacterRepository characterRepository,
    IOfficeRepository officeRepository,
    IFactionRepository factionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCharacterCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken ct)
    {
        var existingCharacter = await characterRepository.GetByNameAsync(command.Request.Name, ct);
        if (existingCharacter is not null)
        {
            throw new DuplicateCharacterException(command.Request.Name);
        }

        Character character = command.Request;

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

        var characterId = characterRepository.Add(character);
        await unitOfWork.SaveChangesAsync(ct);
        return characterId;
    }
}