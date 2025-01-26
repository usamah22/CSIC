using Domain;
using Domain.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

// Persistence/Repositories/EventRepository.cs
public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(int count)
    {
        return await _context.Events
            .Where(e => e.StartDate > DateTime.UtcNow)
            .OrderBy(e => e.StartDate)
            .Take(count)
            .ToListAsync();
    }
    

    public async Task<bool> IsEventFullAsync(Guid eventId)
    {
        var @event = await _context.Events
            .Include(e => e.Bookings)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        return @event != null && @event.Bookings.Count >= @event.Capacity;
    }
    
    public async Task<IReadOnlyList<Event>> SearchEventsAsync(string searchTerm)
    {
        return await _context.Events
            .FullTextSearch(searchTerm, e => e.Title)
            .ToListAsync();
    }
    
    public async Task<EventBooking> GetBookingByIdAsync(Guid bookingId)
    {
        return await _context.EventBookings
            .Include(b => b.Event)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public override async Task<Event> GetByIdAsync(Guid id)
    {
        return await _context.Events
            .Include(e => e.CreatedBy)
            .Include(e => e.Bookings)
            .ThenInclude(b => b.User)
            .Include(e => e.Feedback)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.CreatedBy)
            .Include(e => e.Bookings)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Event>> GetEventsByUserAsync(Guid userId)
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.Bookings)
            .Where(e => e.Bookings.Any(b => b.UserId == userId))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }
}