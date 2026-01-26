namespace FrenchRevolution.Contracts.Models;

public sealed record FactionRequestDto(string Title);

public sealed record FactionResponseDto(
    Guid Id,
    string Title,
    string Description
);

public sealed record FactionSummaryDto(
    Guid Id,
    string Title
);
