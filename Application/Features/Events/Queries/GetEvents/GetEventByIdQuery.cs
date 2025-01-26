using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Queries.GetEvents;

public record GetEventByIdQuery : IRequest<EventDetailDto>
{
    public Guid Id { get; init; }
}