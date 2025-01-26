using MediatR;

namespace Application.Features.Feedback.Commands;

public record DeleteFeedbackCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}