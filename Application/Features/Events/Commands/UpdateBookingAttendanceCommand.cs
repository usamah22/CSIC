using Domain;
using MediatR;

namespace Application.Features.Events.Commands;

public record UpdateBookingAttendanceCommand : IRequest<Unit>
{
    public Guid BookingId { get; init; }
    public BookingStatus Status { get; init; }
}