namespace FrenchRevolution.Application.Abstractions;

public readonly struct Result<T>
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

    public static Result<T> Success(T value) =>
        new Result<T>(true, value, null);

    public static Result<T> Failure(string? error = "An error occurred.") =>
        new Result<T>(false, default, error ?? "An error occurred.");

    public static implicit operator Result<T>(T value) => Success(value);
}