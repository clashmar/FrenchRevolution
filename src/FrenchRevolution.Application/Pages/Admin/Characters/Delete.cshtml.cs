using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Application.Characters.Queries;
using FrenchRevolution.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Admin.Characters;

public class DeleteModel(ISender sender) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public CharacterResponseDto Character { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var character = await sender.Send(new GetCharacterByIdQuery(Id));
        if (character is null)
        {
            return NotFound();
        }

        Character = character;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await sender.Send(new DeleteCharacterCommand(Id));
        return RedirectToPage("Index");
    }
}
