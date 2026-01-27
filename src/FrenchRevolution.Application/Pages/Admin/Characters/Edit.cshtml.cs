using FrenchRevolution.Application.Characters.Commands;
using FrenchRevolution.Contracts.Mapping;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Contracts.Models.Pages;
using FrenchRevolution.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrenchRevolution.Application.Pages.Admin.Characters;

public class EditModel(
    ISender sender,
    ICharacterRepository repository) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CharacterInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var character = await repository.GetByIdAsync(Id);
        if (character is null)
        {
            return NotFound();
        }

        var dto = character.ToResponseDto();

        Input = new CharacterInputModel
        {
            Name = dto.Name,
            Profession = dto.Profession,
            Born = dto.Born,
            Died = dto.Died,
            PortraitUrl = dto.PortraitUrl,
            Offices = dto.Offices.Select(o => new OfficeInput
            {
                Title = o.Title,
                From = o.From,
                To = o.To
            }).ToList(),
            Factions = dto.Factions.Select(f => new FactionInput
            {
                Title = f.Title
            }).ToList()
        };

        return Page();
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

        await sender.Send(new UpdateCharacterCommand(Id, request));

        return RedirectToPage("Index");
    }
}
