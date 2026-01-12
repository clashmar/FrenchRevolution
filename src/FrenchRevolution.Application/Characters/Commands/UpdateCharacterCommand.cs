using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Entities;
using MediatR;

namespace FrenchRevolution.Application.Characters.Commands;

public record UpdateCharacterCommand(Guid Id, CharacterRequestDto Character) : IRequest<bool>;