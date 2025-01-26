using MediatR;

namespace Application.Features.Events.Commands.CreateEventBooking;

public record CreateEventBookingCommand : IRequest<Guid>
{
    public Guid EventId { get; init; }
}
