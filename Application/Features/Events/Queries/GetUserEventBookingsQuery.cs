using Application.DTOs;
using MediatR;

namespace Application.Features.Events.Queries;

public record GetUserEventBookingsQuery : IRequest<List<EventBookingDto>>;