using Application.DTOs;
using Application.Features.Events.Commands;
using Application.Features.Events.Commands.CreateEvent;
using Application.Features.Events.Commands.CreateEventBooking;
using Application.Features.Events.Queries.GetEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EventsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetEvents([FromQuery] GetEventsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<List<EventDto>>> GetUpcomingEvents([FromQuery] int count = 5)
    {
        return await Mediator.Send(new GetUpcomingEventsQuery { Count = count });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDetailDto>> GetEvent(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new GetEventByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return result;
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving event", details = ex.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<Guid>> Create(CreateEventCommand command)
    {
        var eventId = await Mediator.Send(command);
        return Ok(eventId);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> Update(Guid id, UpdateEventCommand command)
    {
        if (id != command.Id)
            return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> UpdateStatus(Guid id, UpdateEventStatusCommand command)
    {
        if (id != command.Id)
            return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/bookings")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateBooking(Guid id, CreateEventBookingCommand command)
    {
        if (id != command.EventId)
            return BadRequest("Event ID mismatch");

        try
        {
            var bookingId = await Mediator.Send(command);
            return Ok(new { bookingId }); // Return in a structured format
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}