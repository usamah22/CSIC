using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Events.Queries.GetEvents;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(
        IEventRepository eventRepository,
        IMapper mapper,
        ILogger<GetEventByIdQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EventDetailDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetEventByIdQuery for EventId: {EventId}", request.Id);
        
        var @event = await _eventRepository.GetByIdAsync(request.Id);
        
        if (@event == null)
        {
            _logger.LogWarning("Event not found for Id: {EventId}", request.Id);
            return null;
        }

        var result = _mapper.Map<EventDetailDto>(@event);
        _logger.LogInformation("Successfully mapped event {EventId} to DTO", request.Id);
        
        return result;
    }
}