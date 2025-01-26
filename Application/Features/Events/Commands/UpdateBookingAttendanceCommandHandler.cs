using Domain;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Events.Commands;

public class UpdateBookingAttendanceCommandHandler : IRequestHandler<UpdateBookingAttendanceCommand, Unit>
{
    private readonly IEventRepository _eventRepository;

    public UpdateBookingAttendanceCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Unit> Handle(UpdateBookingAttendanceCommand request, CancellationToken cancellationToken)
    {
        var booking = await _eventRepository.GetBookingByIdAsync(request.BookingId);
        if (booking == null)
            throw new NotFoundException(nameof(EventBooking), request.BookingId);

        booking.UpdateStatus(request.Status);
        await _eventRepository.UpdateAsync(booking.Event);

        return Unit.Value;
    }
}