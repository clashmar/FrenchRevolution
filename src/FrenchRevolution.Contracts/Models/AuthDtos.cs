namespace FrenchRevolution.Contracts.Models;

public sealed record LoginRequestDto(string Email, string Password);
public record LoginResponseDto(string Token, DateTime ExpiresAt);