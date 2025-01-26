using Domain.Common;

namespace Domain.Events;

public class EventCancelledEvent : DomainEvent
{
    public Guid EventId { get; }
    public string Reason { get; }

    public EventCancelledEvent(Guid eventId, string reason)
    {
        EventId = eventId;
        Reason = reason;
    }
}