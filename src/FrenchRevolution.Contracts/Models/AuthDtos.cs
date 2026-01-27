namespace FrenchRevolution.Contracts.Models;

public sealed record LoginRequestDto(string Email, string Password);
public sealed record LoginResponseDto(string Token);

public sealed record RegisterRequestDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string? DisplayName
);

public sealed record RegisterResponseDto(string UserId, string Email);