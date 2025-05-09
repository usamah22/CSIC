using Domain;

namespace Application.DTOs;

public class EventBookingDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime BookedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public UserDto? User { get; set; }
    public EventDto? Event { get; set; }
}