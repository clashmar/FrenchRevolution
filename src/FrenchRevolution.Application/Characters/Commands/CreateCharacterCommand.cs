using FrenchRevolution.Application.Abstractions;
using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Characters.Commands;

public record CreateCharacterCommand(CharacterRequestDto Request) : IRequest<Guid>;
