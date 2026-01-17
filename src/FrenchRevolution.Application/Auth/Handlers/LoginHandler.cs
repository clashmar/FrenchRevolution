using System.Security.Claims;
using System.Text;
using FrenchRevolution.Application.Abstractions;
using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Application.Auth.Services;
using FrenchRevolution.Application.Config;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace FrenchRevolution.Application.Auth.Handlers;

internal sealed class LoginHandler(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService
) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand command,
        CancellationToken ct)
    {
        var request = command.Request;
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        { 
            return Result<LoginResponseDto>.Failure();
        }
        
        var result = await signInManager.CheckPasswordSignInAsync(
            user, 
            request.Password, 
            lockoutOnFailure: false
            );
 
        if (!result.Succeeded)
        {
            return Result<LoginResponseDto>.Failure();
        }
        
        var roles = await userManager.GetRolesAsync(user);

        var token = tokenService.CreateTokenForUser(user, roles);
        
        return Result<LoginResponseDto>.Success(new LoginResponseDto(token));
    }
}