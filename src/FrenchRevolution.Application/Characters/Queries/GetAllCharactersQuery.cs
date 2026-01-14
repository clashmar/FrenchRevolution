using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Characters.Queries;

public record GetAllCharactersQuery() : IRequest<IEnumerable<CharacterResponseDto>>;