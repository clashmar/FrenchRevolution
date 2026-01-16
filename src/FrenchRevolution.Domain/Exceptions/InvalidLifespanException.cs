namespace FrenchRevolution.Domain.Exceptions;

public sealed class InvalidLifeSpanException(DateTime born, DateTime died)
    : DomainException(
        $"Date of death ({died:yyyy‑MM‑dd}) cannot be earlier than date of birth ({born:yyyy‑MM‑dd})."
        )
{
    public DateTime Born { get; } = born;
    public DateTime Died { get; } = died;
}