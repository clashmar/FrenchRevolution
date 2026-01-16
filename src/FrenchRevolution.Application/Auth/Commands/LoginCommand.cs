using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Application.Abstractions;
using MediatR;

namespace FrenchRevolution.Application.Auth.Commands;

public record LoginCommand(LoginRequestDto Request) : IRequest<Result<LoginResponseDto>>;
