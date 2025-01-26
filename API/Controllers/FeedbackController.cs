using Application.DTOs;
using Application.Features.Feedback.Commands;
using Application.Features.Feedback.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FeedbackController : BaseApiController
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<FeedbackDto>>> GetAllFeedback()
    {
        return await Mediator.Send(new GetAllFeedbackQuery());
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<List<FeedbackDto>>> GetMyFeedback()
    {
        return await Mediator.Send(new GetUserFeedbackQuery());
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateFeedback(Guid id, UpdateFeedbackCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteFeedback(Guid id)
    {
        await Mediator.Send(new DeleteFeedbackCommand { Id = id });
        return NoContent();
    }
}