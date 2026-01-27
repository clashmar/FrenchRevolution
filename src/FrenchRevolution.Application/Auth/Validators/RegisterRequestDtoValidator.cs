using FluentValidation;
using FrenchRevolution.Contracts.Models;

namespace FrenchRevolution.Application.Auth.Validators;

internal sealed class RegisterRequestDtoValidator
    : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .NotNull().NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("Display name must be at most 100 characters.")
            .When(x => x.DisplayName is not null);
    }
}
