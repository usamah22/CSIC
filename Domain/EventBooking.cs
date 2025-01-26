using Domain.Common;
using Domain.Exceptions;

namespace Domain;

public class EventBooking : BaseEntity
{
   public Guid Id { get; private set; }
   public Guid EventId { get; private set; }
   public Guid UserId { get; private set; }
   public BookingStatus Status { get; private set; }
   public DateTime BookedAt { get; private set; }
   public DateTime? CancelledAt { get; private set; }
   
   public Event Event { get; private set; } = null!;
   public User User { get; private set; } = null!;

   private EventBooking() { } // For EF Core

   public static EventBooking Create(
       Guid eventId,
       Guid userId)
   {
       return new EventBooking
       {
           EventId = eventId,
           UserId = userId,
           Status = BookingStatus.Confirmed,
           BookedAt = DateTime.UtcNow
       };
   }

   public void UpdateStatus(BookingStatus newStatus)
   {
       // Cannot update status if booking is already cancelled
       if (Status == BookingStatus.Cancelled)
           throw new DomainException("Cannot update status of a cancelled booking");

       // Cannot change status from Attended or NoShow
       if (Status is BookingStatus.Attended or BookingStatus.NoShow)
           throw new DomainException("Cannot update status after attendance has been recorded");

       Status = newStatus;

       // If cancelling, record the cancellation time
       if (newStatus == BookingStatus.Cancelled)
           CancelledAt = DateTime.UtcNow;
   }

   public void Cancel()
   {
       if (Status == BookingStatus.Cancelled)
           throw new DomainException("Booking is already cancelled");

       if (Status is BookingStatus.Attended or BookingStatus.NoShow)
           throw new DomainException("Cannot cancel booking after attendance has been recorded");

       Status = BookingStatus.Cancelled;
       CancelledAt = DateTime.UtcNow;
   }

   public void MarkAttendance(bool attended)
   {
       if (Status == BookingStatus.Cancelled)
           throw new DomainException("Cannot mark attendance for a cancelled booking");

       if (Status is BookingStatus.Attended or BookingStatus.NoShow)
           throw new DomainException("Attendance has already been recorded");

       Status = attended ? BookingStatus.Attended : BookingStatus.NoShow;
   }
}
public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Attended,
    NoShow
}