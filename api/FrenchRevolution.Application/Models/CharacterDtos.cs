using System.ComponentModel.DataAnnotations;
using FrenchRevolution.Domain.Entities;

namespace FrenchRevolution.Application.Models;

public sealed record CharacterResponse(
    Guid Id,
    string Name,
    string Profession,
    DateTime DateOfBirth,
    DateTime DateOfDeath
)
{
    public static implicit operator CharacterResponse(Character c) =>
        new(c.Id, c.Name, c.Profession, c.DateOfBirth, c.DateOfDeath);
}

public sealed record CharacterRequest(
    [Required] string Name,
    [Required] string Profession,
    [Required] DateTime DateOfBirth,
    [Required] DateTime DateOfDeath
)
{
    public static implicit operator Character(CharacterRequest r) =>
        new(r.Name, r.Profession, r.DateOfBirth, r.DateOfDeath);
}