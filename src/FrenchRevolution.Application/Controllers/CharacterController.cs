using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

/// <summary>
/// API endpoints for managing characters in the French Revolution domain.
/// </summary>
public class CharacterController(ISender sender) : BaseApiController
{
    /// <param name="name">Optional filter by name.</param>
    /// <param name="sortColumn">Column to sort on.</param>
    /// <param name="sortOrder">"asc" or "desc".</param>
    /// <param name="page">Page number (1‑based).</param>
    /// <param name="pageSize">Items per page.</param>
    /// <returns>A paged list of <see cref="CharacterResponseDto"/>.</returns>
    [HttpGet] 
    public async Task<ActionResult<PagedList<CharacterResponseDto>>> GetAll(
        [FromQuery] string? name,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
        )
    {
        var characters = await sender.Send(
            new GetAllCharactersQuery(
                name, 
                sortColumn, 
                sortOrder,
                page,
                pageSize)
            );
        
        return Ok(characters);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharacterResponseDto>> GetById(Guid id)
    {
        var character = await sender.Send(new GetCharacterByIdQuery(id));
        
        return character is null ? 
            NotFound("Unable to find character.") : 
            Ok(character);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<Guid>> Create(CharacterRequestDto request)
    {
        var guid = await sender.Send(new CreateCharacterCommand(request));
        return CreatedAtAction(
            nameof(GetById),             
            new { id = guid },                
            guid); 
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<CharacterResponseDto>> Update(Guid id, CharacterRequestDto request)
    {
        var updated = await sender.Send(new UpdateCharacterCommand(id, request));
        return updated
            ? NoContent()
            : NotFound("Unable to update character.");
    }

    [HttpDelete("{id:guid}")] 
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await sender.Send(new DeleteCharacterCommand(id));
        return deleted
            ? NoContent()
            : NotFound("Unable to delete character.");
    }
}  