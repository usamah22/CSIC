namespace Application.DTOs;

public class FeedbackDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}