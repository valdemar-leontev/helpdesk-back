using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Notifications;

public class DeleteViewedNotificationListCommand : DataCommand
{
    public DeleteViewedNotificationListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    private DateTimeOffset DeletedModeResolver(NotificationListDeletedModes mode)
    {
        var now = DateTimeOffset.UtcNow;

        switch (mode)
        {
            case NotificationListDeletedModes.Day:

                return now.AddDays(-1);
            case NotificationListDeletedModes.Week:

                return now.AddDays(-7);
            case NotificationListDeletedModes.Month:

                return now.AddMonths(-1);
            case NotificationListDeletedModes.Viewed:

                return DateTimeOffset.MinValue;
            default:

                return now;
        }
    }

    public async Task<CommandResponseModel<IEnumerable<NotificationDataModel?>>> DeleteAsync(NotificationListDeletedModes mode)
    {
        var boundaryDate = DeletedModeResolver(mode);

        var viewedNotificationList = await AppDatabaseContext
            .Set<NotificationDataModel>()
            .Where(n => n.IsRead == true && n.RecipientUserId == UserId && n.CreationDate > boundaryDate)
            .ToArrayAsync();

        foreach (var viewedNotification in viewedNotificationList)
        {
            viewedNotification.IsDeleted = true;
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<NotificationDataModel?>>
            (
                errorDetail: $"Сущности '{Description(typeof(NotificationDataModel))}' не были удалены."
            );
        }

        return CommandResponse<IEnumerable<NotificationDataModel?>>
        (
            viewedNotificationList
        );
    }
}