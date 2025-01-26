using Application.Common.Interfaces;
using Domain;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateEventCommandHandler(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = Event.Create(
            request.Title,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.Capacity,
            request.Location,
            _currentUserService.UserId
        );

        await _eventRepository.AddAsync(entity);
        return entity.Id;
    }
}