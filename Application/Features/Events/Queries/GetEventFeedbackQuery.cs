using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Queries;

public record GetEventFeedbackQuery : IRequest<List<EventFeedbackDto>>
{
    public Guid EventId { get; init; }
}