using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators;

// AbstractValidator<ClasseDeEntradaParaSerValidado> É a DTO de entrada
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Campo 'Username' não pode estar vazio");
    }
}
