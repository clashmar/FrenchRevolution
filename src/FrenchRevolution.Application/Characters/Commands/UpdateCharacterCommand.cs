using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Characters.Commands;

public record UpdateCharacterCommand(Guid Id, CharacterRequestDto Request) : IRequest<bool>;