using System.ComponentModel.DataAnnotations;
using FrenchRevolution.Domain.Entities;

namespace FrenchRevolution.Contracts.Models;

public sealed record CharacterResponseDto(
    Guid Id,
    string Name,
    string Profession,
    DateTime DateOfBirth,
    DateTime DateOfDeath
)
{
    public static implicit operator CharacterResponseDto(Character c) =>
        new(c.Id, c.Name, c.Profession, c.DateOfBirth, c.DateOfDeath);
}

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