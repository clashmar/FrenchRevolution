using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Contracts.Mapping;

public static class CharacterResponseMapper
{
    public static CharacterResponseDto ToResponseDto(this Character character)
    {
        return new CharacterResponseDto(
            character.Id,
            character.Name,
            character.Profession,
            character.Born,
            character.Died,
            character.Portrait.Url,
            character.CharacterOffices
                .Select(cr => new OfficeResponseDto(
                    cr.Office.Title,
                    cr.From,
                    cr.To))
                .ToList(),
            character.CharacterFactions
                .Select(cf => new FactionSummaryDto(
                    cf.Faction.Id,
                    cf.Faction.Title))
                .ToList()
        );
    }
}