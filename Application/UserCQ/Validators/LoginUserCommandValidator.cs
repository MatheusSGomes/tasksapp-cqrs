using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Campo 'Email' não pode estar vazio")
            .EmailAddress().WithMessage("Email inválido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Campo 'Password' não pode estar vazio");
    }
}
