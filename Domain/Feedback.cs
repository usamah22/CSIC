using Domain.Common;
using Domain.Exceptions;

namespace Domain;

public class Feedback : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public bool IsPublic { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Event Event { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private Feedback() { } // For EF Core

    public static Feedback Create(
        Guid eventId,
        Guid userId,
        int rating,
        string comment,
        bool isPublic = true)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(comment))
            throw new DomainException("Comment cannot be empty");

        return new Feedback
        {
            EventId = eventId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            IsPublic = isPublic,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(comment))
            throw new DomainException("Comment cannot be empty");

        Rating = rating;
        Comment = comment;
    }

    public void ToggleVisibility()
    {
        IsPublic = !IsPublic;
    }
}