using Application.Common.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Events.Commands.CreateEventBooking;

public class CreateEventBookingCommandHandler : IRequestHandler<CreateEventBookingCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateEventBookingCommandHandler> _logger;

    public CreateEventBookingCommandHandler(
        IEventRepository eventRepository,
        ICurrentUserService currentUserService,
        ILogger<CreateEventBookingCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateEventBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting booking creation for event {EventId}", request.EventId);
            
            var @event = await _eventRepository.GetByIdAsync(request.EventId);
            if (@event == null)
            {
                _logger.LogWarning("Event {EventId} not found", request.EventId);
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            _logger.LogInformation("Event found. Status: {Status}, Capacity: {Capacity}", 
                @event.Status, @event.Capacity);

            var booking = EventBooking.Create(request.EventId, _currentUserService.UserId);
            @event.AddBooking(booking);

            _logger.LogInformation("Booking created. Saving changes...");
            await _eventRepository.UpdateAsync(@event);
            _logger.LogInformation("Booking {BookingId} saved successfully", booking.Id);

            return booking.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create booking for event {EventId}", request.EventId);
            throw;
        }
    }
}