using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Contracts.Models.Pages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Admin.Characters;

public class CreateModel(ISender sender) : PageModel
{
    [BindProperty]
    public CharacterInputModel Input { get; set; } = new();

    public void OnGet()
    {
        Input.Born = new DateTime(1750, 1, 1);
        Input.Died = new DateTime(1793, 1, 1);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var offices = Input.Offices?
            .Where(o => !string.IsNullOrWhiteSpace(o.Title))
            .Select(o => new OfficeRequestDto(o.Title, o.From, o.To))
            .ToList() ?? [];

        var factions = Input.Factions?
            .Where(f => !string.IsNullOrWhiteSpace(f.Title))
            .Select(f => new FactionRequestDto(f.Title))
            .ToList() ?? [];

        var request = new CharacterRequestDto(
            Input.Name,
            Input.Profession,
            Input.Born,
            Input.Died,
            Input.PortraitUrl ?? string.Empty,
            offices,
            factions);

        await sender.Send(new CreateCharacterCommand(request));

        return RedirectToPage("Index");
    }
}
