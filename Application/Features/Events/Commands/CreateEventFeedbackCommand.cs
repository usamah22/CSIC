using MediatR;

namespace Application.Features.Events.Commands;

public record CreateEventFeedbackCommand : IRequest<Unit>
{
    public Guid EventId { get; init; }
    public int Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
}