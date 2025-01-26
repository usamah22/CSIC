using Domain;

namespace Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public EventStatus Status { get; set; }
    public int CurrentBookings { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
}
