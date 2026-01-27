using FrenchRevolution.Application.Abstractions;
using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FrenchRevolution.Application.Auth.Handlers;

internal sealed class RegisterHandler(
    UserManager<ApplicationUser> userManager
) : IRequestHandler<RegisterCommand, Result<RegisterResponseDto>>
{
    public async Task<Result<RegisterResponseDto>> Handle(
        RegisterCommand command,
        CancellationToken ct)
    {
        var request = command.Request;

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return Result<RegisterResponseDto>.Failure("A user with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            DisplayName = request.DisplayName
        };

        var result = await userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result<RegisterResponseDto>.Failure(errors);
        }

        await userManager.AddToRoleAsync(user, Roles.Member);

        return Result<RegisterResponseDto>.Success(new RegisterResponseDto(user.Id, user.Email!));
    }
}
