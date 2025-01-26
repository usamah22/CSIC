using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Commands;

public class CancelEventBookingCommandHandler : IRequestHandler<CancelEventBookingCommand, Unit>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public CancelEventBookingCommandHandler(
        IEventRepository eventRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CancelEventBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _eventRepository.GetBookingByIdAsync(request.BookingId);
        if (booking == null)
            throw new NotFoundException(nameof(EventBooking), request.BookingId);

        if (booking.UserId != _currentUserService.UserId)
            throw new ForbiddenAccessException();

        booking.Cancel();
        await _eventRepository.UpdateAsync(booking.Event);

        return Unit.Value;
    }
}