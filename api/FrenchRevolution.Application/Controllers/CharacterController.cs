using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

public class CharacterController(IMediator mediator) : BaseApiController
{
    [HttpGet] 
    public async Task<ActionResult<List<CharacterResponseDto>>> GetAll()
    {
        var characters = await mediator.Send(new GetAllCharactersQuery());
        return Ok(characters.Select(c => c).ToList());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharacterResponseDto>> GetById(Guid id)
    {
        var character = await mediator.Send(new GetCharacterByIdQuery(id));
        
        return character is null ? 
            NotFound("Unable to find character.") : 
            Ok(character);
    }

    [HttpPost]
    public async Task<ActionResult<CharacterResponseDto>> Create(CharacterRequestDto request)
    {
        var character = await mediator.Send(new CreateCharacterCommand(request));
        return character is null 
            ? BadRequest("Unable to create character.")
            : CreatedAtAction(
                nameof(GetById), 
                new { id = character.Id },
                character);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CharacterResponseDto>> Update(Guid id, CharacterRequestDto request)
    {
        var character = await mediator.Send(new UpdateCharacterCommand(id, request));
        return character is null
            ? NotFound("Unable to update character.")
            : Ok(character);
    }

    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await mediator.Send(new DeleteCharacterCommand(id));
        return deleted
            ? NoContent()
            : NotFound("Unable to delete character.");
    }
}  