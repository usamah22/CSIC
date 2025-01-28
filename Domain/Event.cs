using Domain.Common;
using Domain.Exceptions;

namespace Domain;

public class Event : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int Capacity { get; private set; }
    public string Location { get; private set; }
    public EventStatus Status { get; private set; }
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    
    public User CreatedBy { get; private set; } = null!;
    
    private readonly List<EventBooking> _bookings = new();
    public IReadOnlyCollection<EventBooking> Bookings => _bookings.AsReadOnly();
    
    private readonly List<Feedback> _feedback = new();
    public IReadOnlyCollection<Feedback> Feedback => _feedback.AsReadOnly();

    private Event() { } // For EF Core

    public static Event Create(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        int capacity,
        string location,
        Guid createdById)
    {
        return new Event
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            Capacity = capacity,
            Location = location,
            Status = EventStatus.Draft,
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool IsAvailableForBooking()
    {
        if (Status != EventStatus.Published)
            return false;

        if (StartDate <= DateTime.UtcNow)
            return false;

        if (Status == EventStatus.Cancelled || Status == EventStatus.Completed)
            return false;

        if (_bookings.Count(b => b.Status != BookingStatus.Cancelled) >= Capacity)
            return false;

        return true;
    }

    public void AddBooking(EventBooking booking)
    {
        // Check if event is published
        if (Status != EventStatus.Published)
            throw new DomainException($"Cannot book event with status {Status}. Event must be published.");

        // Check if event hasn't started yet
        if (StartDate <= DateTime.UtcNow)
            throw new DomainException("Cannot book this event as it has already started or ended.");

        // Check if event isn't cancelled
        if (Status == EventStatus.Cancelled)
            throw new DomainException("Cannot book a cancelled event.");

        // Check if event isn't completed
        if (Status == EventStatus.Completed)
            throw new DomainException("Cannot book a completed event.");

        // Check capacity
        var activeBookings = _bookings.Count(b => b.Status != BookingStatus.Cancelled);
        if (activeBookings >= Capacity)
            throw new DomainException($"Event is full. Capacity: {Capacity}, Current bookings: {activeBookings}");

        // Check for existing active booking for the same user
        if (_bookings.Any(b => 
                b.UserId == booking.UserId && 
                b.Status != BookingStatus.Cancelled))
        {
            throw new DomainException("You have already booked this event.");
        }

        _bookings.Add(booking);
    }

    public void UpdateStatus(EventStatus newStatus)
    {
        if (newStatus == Status)
            return;

        // Add validation rules for status transitions
        switch (newStatus)
        {
            case EventStatus.Published:
                if (Status != EventStatus.Draft)
                    throw new DomainException("Only draft events can be published.");
                break;
            case EventStatus.Cancelled:
                if (Status == EventStatus.Completed)
                    throw new DomainException("Completed events cannot be cancelled.");
                break;
            case EventStatus.Completed:
                if (Status == EventStatus.Cancelled)
                    throw new DomainException("Cancelled events cannot be completed.");
                if (EndDate > DateTime.UtcNow)
                    throw new DomainException("Cannot complete an event before its end date.");
                break;
        }

        Status = newStatus;
        LastModifiedAt = DateTime.UtcNow;
    }
}

public enum EventStatus
{
    Draft,
    Published,
    Cancelled,
    Completed
}