using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Commands;

public record GetUpcomingEventsQuery : IRequest<List<EventDto>>
{
    public int Count { get; init; } = 5;
}