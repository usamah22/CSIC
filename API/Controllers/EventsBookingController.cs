using Application.DTOs;
using Application.Features.Events.Commands;
using Application.Features.Events.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EventBookingsController : BaseApiController
{
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<List<EventBookingDto>>> GetMyBookings()
    {
        return await Mediator.Send(new GetUserEventBookingsQuery());
    }

    [HttpPut("{id}/cancel")]
    [Authorize]
    public async Task<ActionResult> CancelBooking(Guid id)
    {
        await Mediator.Send(new CancelEventBookingCommand { BookingId = id });
        return NoContent();
    }

    [HttpPut("{id}/attendance")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> UpdateAttendance(Guid id, UpdateBookingAttendanceCommand command)
    {
        if (id != command.BookingId)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }
}