using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

internal sealed class CreateCharacterHandler(
    ICharacterRepository characterRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCharacterCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken ct)
    {
        Character character = command.Request;
        
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
        
        var characterId = characterRepository.Add(character);
        await unitOfWork.SaveChangesAsync(ct);
        return characterId;
    }
}