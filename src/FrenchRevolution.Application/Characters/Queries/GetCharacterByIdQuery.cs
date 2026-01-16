using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Characters.Queries;

public record GetCharacterByIdQuery(Guid Id) : IRequest<CharacterResponseDto?>;