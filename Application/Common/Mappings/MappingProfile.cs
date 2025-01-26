using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(d => d.CurrentBookings, 
                opt => opt.MapFrom(s => s.Bookings.Count));
            
        CreateMap<Event, EventDetailDto>()
            .ForMember(d => d.CurrentBookings, 
                opt => opt.MapFrom(s => s.Bookings.Count));
            
        CreateMap<EventBooking, EventBookingDto>();
        
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(d => d.EventTitle, 
                opt => opt.MapFrom(s => s.Event.Title))
            .ForMember(d => d.UserFullName, 
                opt => opt.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));
    }
}