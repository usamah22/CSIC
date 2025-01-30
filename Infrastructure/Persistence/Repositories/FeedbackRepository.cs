using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbackRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Feedback>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Feedback
            .AsNoTracking()
            .Include(f => f.Event)
            .Include(f => f.User)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Feedback>> GetEventFeedbackAsync(Guid eventId)
    {
        return await _context.Feedback
            .AsNoTracking()
            .Include(f => f.User)
            .Include(f => f.Event)
            .Where(f => f.EventId == eventId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<Feedback?> GetUserEventFeedbackAsync(Guid eventId, Guid userId)
    {
        return await _context.Feedback
            .AsNoTracking()
            .Include(f => f.Event)
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.EventId == eventId && f.UserId == userId);
    }

    public override async Task<Feedback?> GetByIdAsync(Guid id)
    {
        return await _context.Feedback
            .Include(f => f.Event)
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public override async Task<IReadOnlyList<Feedback>> GetAllAsync()
    {
        return await _context.Feedback
            .AsNoTracking()
            .Include(f => f.Event)
            .Include(f => f.User)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }
}