using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Commands;

public class UpdateFeedbackCommandHandler : IRequestHandler<UpdateFeedbackCommand, Unit>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        ICurrentUserService currentUserService)
    {
        _feedbackRepository = feedbackRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(request.Id);
        if (feedback == null)
            throw new NotFoundException(nameof(Feedback), request.Id);

        if (feedback.UserId != _currentUserService.UserId)
            throw new ForbiddenAccessException();

        feedback.Update(request.Rating, request.Comment);
        await _feedbackRepository.UpdateAsync(feedback);

        return Unit.Value;
    }
}