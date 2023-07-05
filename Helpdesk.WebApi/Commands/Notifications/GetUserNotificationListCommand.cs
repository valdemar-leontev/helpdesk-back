using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Notifications;

public sealed class GetUserNotificationListCommand : DataCommand
{
    public GetUserNotificationListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<NotificationModel>>> GetAsync()
    {
        var userNotifications = await AppDatabaseContext
            .Set<NotificationDataModel>()
            .Include(n => n.RequirementLinkNotification)
            .Where(n => n.RecipientUserId == UserId && !n.IsDeleted)
            .ProjectTo<NotificationModel>(Mapper.ConfigurationProvider)
            .OrderByDescending(n => n.CreationDate)
            .ToArrayAsync();

        return CommandResponse<IEnumerable<NotificationModel>>
        (
            userNotifications
        );
    }
}