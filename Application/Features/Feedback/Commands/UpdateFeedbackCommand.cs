using MediatR;

namespace Application.Features.Feedback.Commands;

public record UpdateFeedbackCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public int Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
}