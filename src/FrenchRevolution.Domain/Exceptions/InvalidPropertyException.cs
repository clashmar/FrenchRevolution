namespace FrenchRevolution.Domain.Exceptions;

public class InvalidPropertyException(string propertyName)
    : DomainException($"The '{propertyName}' property is required.")
{
    public string PropertyName { get; } = propertyName;
}