using Domain;
using Domain.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories;

// Persistence/Repositories/EventRepository.cs
public class EventRepository : BaseRepository<Event>, IEventRepository
{
    
    private readonly ILogger<EventRepository> _logger;

    public EventRepository(ApplicationDbContext context,  ILogger<EventRepository> logger) : base(context)
    {
        _logger = logger;
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

    // For read-only operations
    public async Task<Event> GetByIdReadOnlyAsync(Guid id)
    {
        return await _context.Events
            .AsSplitQuery()
            .AsNoTracking()
            .Include(e => e.CreatedBy)
            .Include(e => e.Bookings)
            .ThenInclude(b => b.User)
            .Include(e => e.Feedback)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    public override async Task<Event> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _context.Events
                .AsSplitQuery()
                // Remove AsNoTracking() here
                .Include(e => e.CreatedBy)
                .Include(e => e.Bookings)
                .ThenInclude(b => b.User)
                .Include(e => e.Feedback)
                .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found", id);
                return null;
            }

            _logger.LogInformation("Successfully retrieved event {EventId} with {BookingCount} bookings and {FeedbackCount} feedback items",
                id, 
                result.Bookings?.Count ?? 0,
                result.Feedback?.Count ?? 0);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event with ID {EventId}", id);
            throw;
        }
    }

    public override async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .AsSplitQuery() 
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
    
    public override async Task UpdateAsync(Event entity)
    {
        try
        {
            _logger.LogInformation("Updating event {EventId}", entity.Id);
            
            // Get the entry
            var entry = _context.Entry(entity);
            
            // Mark as modified
            entry.State = EntityState.Modified;
            
            // For new bookings, mark them as Added
            foreach (var booking in entity.Bookings.Where(b => _context.Entry(b).State == EntityState.Detached))
            {
                _context.EventBookings.Add(booking);
            }

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Event {EventId} updated successfully", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}: {Message}", entity.Id, ex.Message);
            throw;
        }
    }
}