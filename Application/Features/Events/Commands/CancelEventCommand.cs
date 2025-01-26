using MediatR;

namespace Application.Features.Events.Commands;

public record CancelEventCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
}