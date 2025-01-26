using Application.Common.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Commands.CreateEventBooking;

public class CreateEventBookingCommandHandler : IRequestHandler<CreateEventBookingCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateEventBookingCommandHandler(
        IEventRepository eventRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateEventBookingCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        
        if (@event == null)
            throw new NotFoundException(nameof(Event), request.EventId);

        // Check if event is available for booking
        if (@event.Status != EventStatus.Published)
            throw new DomainException("Event is not available for booking.");

        // Check if event is full
        var isEventFull = await _eventRepository.IsEventFullAsync(request.EventId);
        if (isEventFull)
            throw new DomainException("Event is already full.");

        // Check if user already has a booking
        var existingBooking = @event.Bookings.FirstOrDefault(b => b.UserId == _currentUserService.UserId);
        if (existingBooking != null)
            throw new DomainException("You have already booked this event.");

        // Create booking
        var booking = EventBooking.Create(request.EventId, _currentUserService.UserId);
        @event.AddBooking(booking);

        await _eventRepository.UpdateAsync(@event);

        return booking.Id;
    }
}