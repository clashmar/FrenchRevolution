using FluentValidation;
using FrenchRevolution.Application.Characters.Commands;

namespace FrenchRevolution.Application.Characters.Validators;

internal sealed class CreateCharacterCommandValidator
    : AbstractValidator<CreateCharacterCommand>
{
    public CreateCharacterCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(new CharacterRequestDtoValidator());
    }
}

internal sealed class UpdateCharacterCommandValidator
    : AbstractValidator<UpdateCharacterCommand>
{
    public UpdateCharacterCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(new CharacterRequestDtoValidator());
    }
}