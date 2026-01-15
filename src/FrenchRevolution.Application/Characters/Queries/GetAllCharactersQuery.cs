using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Characters.Queries;

public record GetAllCharactersQuery(
    string? Name = null,
    string? SortColumn = null,
    string? SortOrder = null,
    int Page = 1,
    int PageSize = 20
    ) : IRequest<PagedList<CharacterResponseDto>>;