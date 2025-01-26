/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class NotificationsController : BaseApiController
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        return await Mediator.Send(new GetUserNotificationsQuery());
    }

    [HttpGet("unread")]
    [Authorize]
    public async Task<ActionResult<List<NotificationDto>>> GetUnreadNotifications()
    {
        return await Mediator.Send(new GetUnreadNotificationsQuery());
    }

    [HttpPut("{id}/read")]
    [Authorize]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        await Mediator.Send(new MarkNotificationAsReadCommand { NotificationId = id });
        return NoContent();
    }

    [HttpPut("read-all")]
    [Authorize]
    public async Task<ActionResult> MarkAllAsRead()
    {
        await Mediator.Send(new MarkAllNotificationsAsReadCommand());
        return NoContent();
    }
}*/