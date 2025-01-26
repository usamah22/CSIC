using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Queries;

public record GetEventBookingsQuery : IRequest<List<EventBookingDto>>
{
    public Guid EventId { get; init; }
}