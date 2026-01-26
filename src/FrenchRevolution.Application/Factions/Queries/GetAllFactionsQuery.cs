using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Factions.Queries;

public record GetAllFactionsQuery : IRequest<IReadOnlyList<FactionResponseDto>>;

internal sealed class GetAllFactionsHandler(
    IFactionRepository factionRepository
) : IRequestHandler<GetAllFactionsQuery, IReadOnlyList<FactionResponseDto>>
{
    public async Task<IReadOnlyList<FactionResponseDto>> Handle(
        GetAllFactionsQuery query,
        CancellationToken ct)
    {
        var factions = await factionRepository.GetAllAsync(ct);
        return factions.Select(f => f.ToResponseDto()).ToList();
    }
}
