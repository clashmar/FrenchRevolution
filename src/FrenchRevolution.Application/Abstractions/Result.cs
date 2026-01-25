namespace FrenchRevolution.Application.Abstractions;

public interface IResultType
{
    bool IsFailure { get; }
    string? Error { get; }
}

public readonly struct Result<T> : IResultType
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(string? error = "An error occurred") =>
        new(false, default, error ?? "An error occurred");

    public static implicit operator Result<T>(T value) => Success(value);
}