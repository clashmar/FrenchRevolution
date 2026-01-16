using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

internal sealed class CreateCharacterHandler(
    ICharacterRepository characterRepository,
    IOfficeRepository officeRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCharacterCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken ct)
    {
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
        
        var characterId = characterRepository.Add(character);
        await unitOfWork.SaveChangesAsync(ct);
        return characterId;
    }
}