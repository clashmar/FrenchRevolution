using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

public class CharacterController(ISender sender) : BaseApiController
{
    [HttpGet] 
    public async Task<ActionResult<List<CharacterResponseDto>>> GetAll()
    {
        var characters = await sender.Send(new GetAllCharactersQuery());
        return Ok(characters.Select(c => c).ToList());
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
    public async Task<ActionResult<Guid>> Create(CharacterRequestDto request)
    {
        var guid = await sender.Send(new CreateCharacterCommand(request));
        return CreatedAtAction(
            nameof(GetById),             
            new { id = guid },                
            guid); 
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CharacterResponseDto>> Update(Guid id, CharacterRequestDto request)
    {
        var updated = await sender.Send(new UpdateCharacterCommand(id, request));
        return updated
            ? NoContent()
            : NotFound("Unable to update character.");
    }

    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await sender.Send(new DeleteCharacterCommand(id));
        return deleted
            ? NoContent()
            : NotFound("Unable to delete character.");
    }
}  