using Domain.Common;

namespace Domain;

public class Notification : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    
    public User User { get; private set; }

    private Notification() { } // For EF Core

    public static Notification Create(
        Guid userId,
        string title,
        string message,
        NotificationType type)
    {
        return new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }
}

public enum NotificationType
{
    EventReminder,
    EventCancellation,
    NewJobPosting,
    EventFeedbackRequest,
    SystemNotification
}