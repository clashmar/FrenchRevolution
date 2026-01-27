using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

/// <summary>
/// API endpoints for authentication.
/// </summary>
public class AuthController(ISender sender) : BaseApiController
{
    /// <param name="request">The login credentials.</param>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        LoginRequestDto request
        )
    {
        var result = await sender.Send(new LoginCommand(request));

        return result.IsSuccess
            ? Ok(result.Value)
            : Unauthorized("Invalid credentials.");
    }

    /// <param name="request">The registration details.</param>
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register(
        RegisterRequestDto request
        )
    {
        var result = await sender.Send(new RegisterCommand(request));

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}

