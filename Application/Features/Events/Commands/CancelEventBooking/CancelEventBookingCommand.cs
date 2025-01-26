using MediatR;

namespace Application.Features.Events.Commands;

public record CancelEventBookingCommand : IRequest<Unit>
{
    public Guid BookingId { get; init; }
}