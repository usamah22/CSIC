using Application.Features.Events.Commands.CreateEvent;
using FluentValidation;

namespace Application.Features.Events.Commands;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(v => v.StartDate).WithMessage("End date must be after start date");

        RuleFor(v => v.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0");

        RuleFor(v => v.Location)
            .NotEmpty().WithMessage("Location is required");
    }
}