namespace FrenchRevolution.Domain.Exceptions;

public sealed class InvalidPortraitException(string url) 
    : DomainException($"'{url}' is not a valid portrait Url.");