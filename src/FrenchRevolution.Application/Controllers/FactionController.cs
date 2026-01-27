using FrenchRevolution.Application.Factions.Queries;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

/// <summary>
/// API endpoints for managing factions.
/// </summary>
public class FactionController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FactionResponseDto>>> GetAll()
    {
        var factions = await sender.Send(new GetAllFactionsQuery());
        return Ok(factions);
    }

    /// <param name="id">The unique identifier of the faction.</param>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
    public async Task<ActionResult<FactionResponseDto>> GetById(Guid id)
    {
        var faction = await sender.Send(new GetFactionByIdQuery(id));

        return faction is null
            ? NotFound("Unable to find faction.")
            : Ok(faction);
    }
}
