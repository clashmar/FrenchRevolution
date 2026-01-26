using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Repositories;
using MediatR;

namespace FrenchRevolution.Application.Factions.Queries;

public record GetFactionByIdQuery(Guid Id) : IRequest<FactionResponseDto?>;

internal sealed class GetFactionByIdHandler(
    IFactionRepository factionRepository
) : IRequestHandler<GetFactionByIdQuery, FactionResponseDto?>
{
    public async Task<FactionResponseDto?> Handle(
        GetFactionByIdQuery query,
        CancellationToken ct)
    {
        var faction = await factionRepository.GetByIdAsync(query.Id, ct);
        return faction?.ToResponseDto();
    }
}
