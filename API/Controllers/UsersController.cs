using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Features.Users.Queries;
using Application.Features.Users.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        // This will get all users from the database
        var users = await Mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserCommand command)
    {
        // Create a new user with specified email, password and role
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id}/role")]
    public async Task<ActionResult> UpdateUserRole(string id, [FromBody] UpdateUserRoleCommand command)
    {
        if (id != command.UserId)
        {
            return BadRequest("ID in the URL does not match ID in the request body");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        await Mediator.Send(new DeleteUserCommand { UserId = id });
        return NoContent();
    }
}