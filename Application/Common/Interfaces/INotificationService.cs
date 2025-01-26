using Domain;

namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(Guid userId, string title, string message, NotificationType type);
    Task MarkAsReadAsync(Guid notificationId);
}