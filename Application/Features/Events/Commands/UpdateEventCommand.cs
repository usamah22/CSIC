using MediatR;

namespace Application.Features.Events.Commands;

public record UpdateEventCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Capacity { get; init; }
    public string Location { get; init; } = string.Empty;
}