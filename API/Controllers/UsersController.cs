/*using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        return await Mediator.Send(new GetUsersQuery());
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        return await Mediator.Send(new GetCurrentUserQuery());
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult> UpdateProfile(UpdateUserProfileCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}*/