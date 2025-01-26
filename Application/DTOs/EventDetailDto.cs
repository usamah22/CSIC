namespace Application.DTOs;

public class EventDetailDto : EventDto
{
    public List<EventBookingDto> Bookings { get; set; } = new();
}