using Domain;
using MediatR;

namespace Application.Features.Events.Commands;

public record UpdateEventStatusCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public EventStatus Status { get; init; }
}