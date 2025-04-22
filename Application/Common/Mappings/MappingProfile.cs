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
                opt => opt.MapFrom(s => s.Bookings.Count(b => b.Status != BookingStatus.Cancelled)))
            .ForMember(d => d.CreatedById,
                opt => opt.MapFrom(s => s.CreatedById))
            .ForMember(d => d.CreatedAt,
                opt => opt.MapFrom(s => s.CreatedAt));

        CreateMap<Event, EventDetailDto>()
            .ForMember(d => d.CurrentBookings,
                opt => opt.MapFrom(s => s.Bookings.Count(b => b.Status != BookingStatus.Cancelled)));

        CreateMap<User, UserDto>();

        CreateMap<EventBooking, EventBookingDto>();
        
        
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(d => d.EventTitle, opt => opt.MapFrom(s => s.Event.Title))
            .ForMember(d => d.UserFullName, opt => 
                opt.MapFrom(s => string.IsNullOrWhiteSpace(s.User.FullName) 
                    ? s.User.Email 
                    : s.User.FullName));
        
        CreateMap<Domain.ContactMessage, DTOs.ContactMessageDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}