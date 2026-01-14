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
            character.DateOfBirth,
            character.DateOfDeath
        );
    }
}