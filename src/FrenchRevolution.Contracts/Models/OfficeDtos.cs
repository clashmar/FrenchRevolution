namespace FrenchRevolution.Contracts.Models;

public sealed record OfficeRequestDto(
    string Title, 
    DateTime From, 
    DateTime To
    );
    
public sealed record OfficeResponseDto(
    string Title,
    DateTime From,
    DateTime To
    );