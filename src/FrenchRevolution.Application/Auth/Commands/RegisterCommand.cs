using FrenchRevolution.Application.Abstractions;
using FrenchRevolution.Contracts.Models;
using MediatR;

namespace FrenchRevolution.Application.Auth.Commands;

public record RegisterCommand(RegisterRequestDto Request) : IRequest<Result<RegisterResponseDto>>;
