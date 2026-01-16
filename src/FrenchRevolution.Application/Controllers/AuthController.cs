using FrenchRevolution.Application.Auth.Commands;
using FrenchRevolution.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

public class AuthController(ISender sender) : BaseApiController
{
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
}

