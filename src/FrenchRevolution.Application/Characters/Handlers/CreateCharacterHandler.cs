using FrenchRevolution.Application.Abstractions;
using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Characters.Handlers;

internal sealed class CreateCharacterHandler(
    ICharacterRepository repository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCharacterCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCharacterCommand command,
        CancellationToken cancellationToken)
    {
        var result = repository.Add(command.Request);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}