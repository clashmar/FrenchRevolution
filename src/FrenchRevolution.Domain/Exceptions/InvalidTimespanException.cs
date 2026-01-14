namespace FrenchRevolution.Domain.Exceptions;

public sealed class InvalidTimeSpanException(DateTime first, DateTime second)
    : DomainException(
        $"First date: ({first:yyyy‑MM‑dd}) cannot be later than first date: ({second:yyyy‑MM‑dd})."
        )
{
    public DateTime First { get; } = first;
    public DateTime Second { get; } = second;
}