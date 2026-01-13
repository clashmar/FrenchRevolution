namespace FrenchRevolution.Domain.Exceptions;

public sealed class InvalidLifeSpanException(DateTime dateOfBirth, DateTime dateOfDeath)
    : DomainException(
        $"Date of death ({dateOfDeath:yyyy‑MM‑dd}) cannot be earlier than date of birth ({dateOfBirth:yyyy‑MM‑dd})."
        )
{
    public DateTime DateOfBirth { get; } = dateOfBirth;
    public DateTime DateOfDeath { get; } = dateOfDeath;
}