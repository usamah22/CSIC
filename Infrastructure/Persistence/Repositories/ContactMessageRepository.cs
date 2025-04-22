using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories;

public class ContactMessageRepository : BaseRepository<ContactMessage>, IContactMessageRepository
{
    private readonly ILogger<ContactMessageRepository> _logger;

    public ContactMessageRepository(
        ApplicationDbContext context,
        ILogger<ContactMessageRepository> logger) : base(context)
    {
        _logger = logger;
    }

    public async Task<IReadOnlyList<ContactMessage>> GetUnreadMessagesAsync()
    {
        return await _context.ContactMessages
            .Where(m => !m.IsRead)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        return await _context.ContactMessages
            .CountAsync(m => !m.IsRead);
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
        {
            _logger.LogWarning("Contact message with ID {MessageId} not found", id);
            return;
        }

        message.MarkAsRead();
        await _context.SaveChangesAsync();
        _logger.LogInformation("Marked contact message {MessageId} as read", id);
    }

    public async Task MarkAsUnreadAsync(Guid id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
        {
            _logger.LogWarning("Contact message with ID {MessageId} not found", id);
            return;
        }

        message.MarkAsUnread();
        await _context.SaveChangesAsync();
        _logger.LogInformation("Marked contact message {MessageId} as unread", id);
    }

    public override async Task<IReadOnlyList<ContactMessage>> GetAllAsync()
    {
        return await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }
}