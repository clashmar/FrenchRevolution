using FrenchRevolution.Application.Models;
using FrenchRevolution.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharacterController(ICharacterService characterService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<CharacterResponse>>> GetAllCharacters()
    {
        var characters = await characterService.GetAllAsync();
        return Ok(characters.Select(c => c).ToList());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharacterResponse>> GetCharacterById(Guid id)
    {
        var character = await characterService.GetByIdAsync(id);
        
        return character is null ? 
            NotFound("Unable to find character.") : 
            Ok((CharacterResponse)character);
    }

    [HttpPost]
    public async Task<ActionResult<CharacterResponse>> CreateCharacterAsync(CharacterRequest request)
    {
        var character = await characterService.CreateAsync(request);
        return character is null 
            ? BadRequest("Unable to create character.")
            : CreatedAtAction(
                nameof(GetCharacterById), 
                new { id = character.Id },
                (CharacterResponse)character);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CharacterResponse>> UpdateCharacterAsync(Guid id, CharacterRequest request)
    {
        var character = await characterService.UpdateAsync(id, request);
        return character is null
            ? NotFound("Unable to update character.")
            : Ok((CharacterResponse)character);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCharacterAsync(Guid id)
    {
        var deleted = await characterService.DeleteAsync(id);
        return deleted
            ? NoContent()
            : NotFound("Unable to delete character.");
    }
}  