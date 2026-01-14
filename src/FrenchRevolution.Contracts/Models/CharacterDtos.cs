using FrenchRevolution.Domain.Data;

namespace FrenchRevolution.Contracts.Models;

public sealed record CharacterResponseDto(
    Guid Id,
    string Name,
    string Profession,
    DateTime DateOfBirth,
    DateTime DateOfDeath
);

public sealed record CharacterRequestDto(
    string Name,
    string Profession,
    DateTime DateOfBirth,
    DateTime DateOfDeath
)
{
    public static implicit operator Character(CharacterRequestDto r) =>
        new(r.Name, r.Profession, r.DateOfBirth, r.DateOfDeath);
}