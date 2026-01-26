using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Contracts.Mapping;

public static class FactionResponseMapper
{
    public static FactionResponseDto ToResponseDto(this Faction faction)
    {
        return new FactionResponseDto(
            faction.Id,
            faction.Title,
            faction.Description
        );
    }

    public static FactionSummaryDto ToSummaryDto(this Faction faction)
    {
        return new FactionSummaryDto(
            faction.Id,
            faction.Title
        );
    }
}
