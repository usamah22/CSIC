using MediatR;

namespace Application.Features.Events.Commands.CreateEvent;

public record CreateEventCommand : IRequest<Guid>
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Capacity { get; init; }
    public string Location { get; init; } = null!;
}