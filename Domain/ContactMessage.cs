using Domain.Common;
using Domain.ValueObjects;

namespace Domain;

public class ContactMessage : BaseEntity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }

    private ContactMessage() { } // For EF Core

    public static ContactMessage Create(
        string name,
        Email email,
        string message)
    {
        return new ContactMessage
        {
            Name = name,
            Email = email,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void MarkAsUnread()
    {
        IsRead = false;
    }
}