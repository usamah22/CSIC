namespace Domain.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(int count);
    Task<IReadOnlyList<Event>> GetEventsByUserAsync(Guid userId);
    Task<bool> IsEventFullAsync(Guid eventId);
    Task<EventBooking> GetBookingByIdAsync(Guid requestBookingId);
}