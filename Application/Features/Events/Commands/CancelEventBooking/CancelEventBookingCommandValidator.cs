using FluentValidation;

namespace Application.Features.Events.Commands;

public class CancelEventBookingCommandValidator : AbstractValidator<CancelEventBookingCommand>
{
    public CancelEventBookingCommandValidator()
    {
        RuleFor(v => v.BookingId)
            .NotEmpty().WithMessage("BookingId is required.");
    }
}