using FrenchRevolution.Domain.Entities;
using MediatR;

namespace FrenchRevolution.Application.Characters.Queries;

public record GetCharacterByIdQuery(Guid Id) : IRequest<Character?>;