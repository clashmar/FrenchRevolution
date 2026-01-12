using FrenchRevolution.Domain.Entities;
using MediatR;

namespace FrenchRevolution.Application.Characters.Queries;

public record GetAllCharactersQuery() : IRequest<IEnumerable<Character>>;