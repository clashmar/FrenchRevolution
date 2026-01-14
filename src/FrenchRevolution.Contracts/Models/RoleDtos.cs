namespace FrenchRevolution.Contracts.Models;

public sealed record RoleRequestDto(
    string Title, 
    DateTime From, 
    DateTime To
    );
    
public sealed record RoleResponseDto(
    string Title,
    DateTime From,
    DateTime To
    );