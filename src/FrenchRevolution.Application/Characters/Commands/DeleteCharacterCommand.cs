using MediatR;

namespace FrenchRevolution.Application.Characters.Commands;

public record DeleteCharacterCommand(Guid Id) :IRequest<bool>;