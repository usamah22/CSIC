using Application;
using Application.Common.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Events.Commands;

public class UpdateEventStatusCommandHandler : IRequestHandler<UpdateEventStatusCommand, Unit>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UpdateEventStatusCommandHandler> _logger;

    public UpdateEventStatusCommandHandler(
        IEventRepository eventRepository,
        ICurrentUserService currentUserService,
        ILogger<UpdateEventStatusCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateEventStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateEventStatus request for event {EventId}", request.Id);
        
        var @event = await _eventRepository.GetByIdAsync(request.Id);
        
        if (@event == null)
        {
            _logger.LogWarning("Event with ID {EventId} not found", request.Id);
            throw new NotFoundException(nameof(Event), request.Id);
        }
        
        try
        {
            _logger.LogInformation("Updating event {EventId} status from {OldStatus} to {NewStatus}", 
                request.Id, @event.Status, request.Status);
                
            @event.UpdateStatus(request.Status);
            await _eventRepository.UpdateAsync(@event);
            
            _logger.LogInformation("Successfully updated event {EventId} status to {NewStatus}", 
                request.Id, request.Status);
                
            return Unit.Value;
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Domain error occurred when updating event {EventId} status: {ErrorMessage}", 
                request.Id, ex.Message);
                
            throw new ApplicationException(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred when updating event {EventId} status: {ErrorMessage}", 
                request.Id, ex.Message);
                
            throw;
        }
    }
}