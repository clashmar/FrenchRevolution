using FluentValidation;
using FrenchRevolution.Contracts.Models;

namespace FrenchRevolution.Application.Auth.Validators;

internal sealed class LoginRequestDtoValidator 
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
    }
}
