using FluentValidation;
using FrenchRevolution.Contracts.Models;

namespace FrenchRevolution.Application.Characters.Validators;

internal sealed class CharacterRequestDtoValidator 
    : AbstractValidator<CharacterRequestDto>
{
    public CharacterRequestDtoValidator()
    {
        // Name 
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("Name is required.")
            .MinimumLength(4).WithMessage("Name must be at least 3 characters long.");
        
        // Profession
        RuleFor(x => x.Profession)
            .NotNull().NotEmpty().WithMessage("Profession is required.")
            .MinimumLength(4).WithMessage("Profession must be at least 3 characters long.");
        
        // Date of birth
        RuleFor(x => x.Born)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeInPast).WithMessage("Date of birth must be in the past.")
            .LessThan(x => x.Died).WithMessage("Date of birth must be before date of death.");
        
        // Date of death
        RuleFor(x => x.Died)
            .NotEmpty().WithMessage("Date of death is required.")
            .Must(BeInPast).WithMessage("Date of death must be in the past.")
            .GreaterThan(x => x.Born).WithMessage("Date of death must be after date of birth.");

        // Offices
        RuleFor(x => x.Offices)
            .Must(offices => offices
                .All(r => BeInPast(r.From) && BeInPast(r.To)))
                .WithMessage("All office dates must be in the past.");
        
        // Portrait
        RuleFor(x => x.PortraitUrl)
            .NotNull().NotEmpty().WithMessage("Portrait Url is required.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Portrait Url must be a valid Url.");

        // Factions
        RuleForEach(x => x.Factions)
            .ChildRules(faction =>
            {
                faction.RuleFor(f => f.Title)
                    .NotNull().NotEmpty().WithMessage("Faction title is required.");
            });
    }

    private static bool BeInPast(DateTime date)
    {
        return date < DateTime.Today;
    }
}