using Application.Common.Interfaces;
using Domain;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        ApplicationDbContext context,
        ILogger<NotificationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SendNotificationAsync(Guid userId, string title, string message, NotificationType type)
    {
        var notification = Notification.Create(userId, title, message, type);
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.MarkAsRead();
            await _context.SaveChangesAsync();
        }
    }
}