using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Commands;

public class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand, Unit>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public DeleteFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task<Unit> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(request.Id);
        if (feedback == null)
            throw new NotFoundException(nameof(Feedback), request.Id);

        await _feedbackRepository.DeleteAsync(feedback);
        return Unit.Value;
    }
}