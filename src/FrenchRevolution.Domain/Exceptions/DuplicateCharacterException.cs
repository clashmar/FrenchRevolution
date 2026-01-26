namespace FrenchRevolution.Domain.Exceptions;

public class DuplicateCharacterException(string name)
    : DomainException($"A character with the name '{name}' already exists.")
{
    public string Name { get; } = name;
}
