using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Contracts.Models;

public sealed record CharacterResponseDto(
    Guid Id,
    string Name,
    string Profession,
    DateTime Born,
    DateTime Died,
    string PortraitUrl,
    IReadOnlyCollection<OfficeResponseDto> Offices,
    IReadOnlyCollection<FactionSummaryDto> Factions
);

public sealed record CharacterRequestDto(
    string Name,
    string Profession,
    DateTime Born,
    DateTime Died,
    string PortraitUrl,
    IReadOnlyCollection<OfficeRequestDto> Offices,
    IReadOnlyCollection<FactionRequestDto> Factions
    )
{
    public static implicit operator Character(CharacterRequestDto r) =>
        new(r.Name, r.Profession, r.Born, r.Died, new Portrait(r.PortraitUrl));
}