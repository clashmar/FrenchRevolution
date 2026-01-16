using FluentValidation;
using FrenchRevolution.Contracts.Models;
using FrenchRevolution.Infrastructure.Configurations;

namespace FrenchRevolution.Application.Characters.Validators;

public class CharacterRequestDtoValidator : AbstractValidator<CharacterRequestDto>
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

        // Roles
        RuleFor(x => x.Offices)
            .Must(offices => offices
                .All(r => BeInPast(r.From) && BeInPast(r.To)))
                .WithMessage("All office dates must be in the past.");
    }

    private static bool BeInPast(DateTime date)
    {
        return date < DateTime.Today;
    }
}