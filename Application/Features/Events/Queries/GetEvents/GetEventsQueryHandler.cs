using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Queries.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync();
        return _mapper.Map<List<EventDto>>(events);
    }
}