using Application.Common.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Queries;

public class GetUserEventBookingsQueryHandler : IRequestHandler<GetUserEventBookingsQuery, List<EventBookingDto>>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserEventBookingsQueryHandler(
        IEventRepository eventRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<EventBookingDto>> Handle(GetUserEventBookingsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetEventsByUserAsync(_currentUserService.UserId);
        var bookings = events.SelectMany(e => e.Bookings)
            .Where(b => b.UserId == _currentUserService.UserId);
        
        return _mapper.Map<List<EventBookingDto>>(bookings);
    }
}