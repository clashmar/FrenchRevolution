using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Contracts.Models;

public sealed record CharacterResponseDto(
    Guid Id,
    string Name,
    string Profession,
    DateTime Born,
    DateTime Died,
    IReadOnlyCollection<OfficeResponseDto> Offices
);

public sealed record CharacterRequestDto(
    string Name,
    string Profession,
    DateTime Born,
    DateTime Died,
    IReadOnlyCollection<OfficeRequestDto> Offices
    )
{
    public static implicit operator Character(CharacterRequestDto r) =>
        new(r.Name, r.Profession, r.Born, r.Died);
}