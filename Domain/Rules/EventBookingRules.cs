using Domain.Common;

namespace Domain.Rules;

public static class EventBookingRules
{
    public static Result ValidateBooking(Event @event, User user)
    {
        if (@event == null)
            return Result.Failure("Event not found");

        if (user == null)
            return Result.Failure("User not found");

        if (@event.Status != EventStatus.Published)
            return Result.Failure("Cannot book an unpublished event");

        if (@event.StartDate < DateTime.UtcNow)
            return Result.Failure("Cannot book a past event");

        if (@event.Bookings.Count >= @event.Capacity)
            return Result.Failure("Event is full");

        if (@event.Bookings.Any(b => b.UserId == user.Id && b.Status == BookingStatus.Confirmed))
            return Result.Failure("User has already booked this event");

        return Result.Success();
    }
}
