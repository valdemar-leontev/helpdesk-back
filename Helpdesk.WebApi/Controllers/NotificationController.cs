using Helpdesk.WebApi.Commands.Notifications;
using Helpdesk.WebApi.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/notifications")]
public class NotificationController : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromServices] GetUserNotificationListCommand command)
    {
        var notificationsResponse = await command.GetAsync();

        return notificationsResponse.Content is not null
            ? Ok(notificationsResponse.Content)
            : Problem(notificationsResponse.ErrorDetail);
    }

    [HttpPost("{notificationId:int}")]
    public async Task<IActionResult> PostAsync([FromServices] ChangeStateNotificationCommand command, int notificationId)
    {
        var notificationResponse = await command.PostAsync(notificationId);

        return notificationResponse.Content is not null
            ? Ok(notificationResponse.Content)
            : Problem(notificationResponse.ErrorDetail);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteViewedNotificationListCommand command, [FromQuery] NotificationListDeletedModes mode)
    {
        var notificationResponse = await command.DeleteAsync(mode);

        return notificationResponse.Content is not null
            ? Ok(notificationResponse.Content)
            : Problem(notificationResponse.ErrorDetail);
    }
};