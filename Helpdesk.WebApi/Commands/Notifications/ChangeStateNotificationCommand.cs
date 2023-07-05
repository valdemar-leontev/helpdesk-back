using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Notifications;

public sealed class ChangeStateNotificationCommand : DataCommand
{
    public ChangeStateNotificationCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<NotificationDataModel?>> PostAsync(int notificationId)
    {
        var notification = new NotificationDataModel
        {
            Id = notificationId,
            Message = string.Empty,
            IsRead = true
        };

        var entityEntry = AppDatabaseContext.Attach(notification);
        entityEntry.Property(q => q.IsRead).IsModified = true;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<NotificationDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(NotificationDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<NotificationDataModel?>
        (
            content: entityEntry.Entity
        );
    }
}