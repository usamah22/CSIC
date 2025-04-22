using Application.DTOs;
using Application.Features.ContactMessages.Commands;
using Application.Features.ContactMessages.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContactMessagesController : BaseApiController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> Create(CreateContactMessageCommand command)
    {
        try
        {
            var messageId = await Mediator.Send(command);
            return Ok(messageId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating contact message", details = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<List<ContactMessageDto>>> GetAll([FromQuery] bool unreadOnly = false)
    {
        try
        {
            var query = new GetContactMessagesQuery { UnreadOnly = unreadOnly };
            var messages = await Mediator.Send(query);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving contact messages", details = ex.Message });
        }
    }

    [HttpPut("{id}/read")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        try
        {
            await Mediator.Send(new MarkAsReadCommand { Id = id });
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error marking message as read", details = ex.Message });
        }
    }
}