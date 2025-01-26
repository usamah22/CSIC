using FluentValidation;

namespace Application.Features.Events.Commands;

public class UpdateBookingAttendanceCommandValidator : AbstractValidator<UpdateBookingAttendanceCommand>
{
    public UpdateBookingAttendanceCommandValidator()
    {
        RuleFor(v => v.BookingId)
            .NotEmpty().WithMessage("BookingId is required.");

        RuleFor(v => v.Status)
            .IsInEnum().WithMessage("Invalid booking status.");
    }
}