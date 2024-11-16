using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators;

// AbstractValidator<ClasseCujosParametrosPrecisamSerValidados>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email não pode ser vazio.")
            .EmailAddress().WithMessage("Email inválido").WithErrorCode("400");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username não pode estar vazio");
    }
}
