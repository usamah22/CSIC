using FluentValidation;

namespace Application.Features.Events.Commands.CreateEventBooking;

public class CreateEventBookingCommandValidator : AbstractValidator<CreateEventBookingCommand>
{
    public CreateEventBookingCommandValidator()
    {
        RuleFor(v => v.EventId)
            .NotEmpty().WithMessage("EventId is required.");
    }
}