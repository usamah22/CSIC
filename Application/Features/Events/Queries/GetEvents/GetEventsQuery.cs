using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Queries.GetEvents;

public record GetEventsQuery : IRequest<List<EventDto>>;