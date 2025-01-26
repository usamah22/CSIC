using FluentValidation;

namespace Application.Features.Authentication.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}