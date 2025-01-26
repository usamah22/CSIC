using FluentValidation;

namespace Application.Features.Authentication.Commands;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .Must(email => email.EndsWith("@aston.ac.uk"))
            .WithMessage("Must use an Aston University email address");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");
    }
}