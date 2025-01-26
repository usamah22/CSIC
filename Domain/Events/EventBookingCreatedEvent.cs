using Domain.Common;

namespace Domain.Events;

public class EventBookingCreatedEvent : DomainEvent
{
    public Guid EventId { get; }
    public Guid UserId { get; }
    public DateTime BookingDate { get; }

    public EventBookingCreatedEvent(Guid eventId, Guid userId, DateTime bookingDate)
    {
        EventId = eventId;
        UserId = userId;
        BookingDate = bookingDate;
    }
}