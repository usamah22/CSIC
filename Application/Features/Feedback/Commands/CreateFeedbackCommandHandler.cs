using Application.Common.Interfaces;
using Domain;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Commands;

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Guid>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IEventRepository eventRepository,
        ICurrentUserService currentUserService)
    {
        _feedbackRepository = feedbackRepository;
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null)
            throw new NotFoundException(nameof(Event), request.EventId);

        // Check if user has already provided feedback
        var existingFeedback = await _feedbackRepository.GetUserEventFeedbackAsync(
            request.EventId, 
            _currentUserService.UserId
        );

        if (existingFeedback != null)
            throw new InvalidOperationException("You have already provided feedback for this event");
        
        var feedback = Domain.Feedback.Create(
            eventId: request.EventId,
            userId: _currentUserService.UserId,
            rating: request.Rating,
            comment: request.Comment,
            isPublic: true 
        );

        await _feedbackRepository.AddAsync(feedback);

        return feedback.Id;
    }
}