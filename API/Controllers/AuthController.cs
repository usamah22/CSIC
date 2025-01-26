using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

public class AuthController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginCommand command)
    {
        try
        {
            return await Mediator.Send(command);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterCommand command)
    {
        return await Mediator.Send(command);
    }
}