using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Integrations;

public sealed class PutActiveDirectoryUsersCommand : DataCommand
{
    public PutActiveDirectoryUsersCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<int?>> PutAsync(ActiveDirectoryUserModel[] activeDirectoryUsers)
    {
        var users = await AppDatabaseContext
            .Set<UserDataModel>()
            .ToListAsync();

        foreach (var activeDirectoryUser in activeDirectoryUsers)
        {
            var user = users.FirstOrDefault(u => u.ObjectSid == activeDirectoryUser.ObjectSid);

            if (user is null)
            {
                var newUser = Mapper.Map<UserDataModel>(activeDirectoryUser);
                newUser.Profile = new ProfileDataModel
                {
                    FirstName = activeDirectoryUser.FirstName,
                    LastName = activeDirectoryUser.LastName
                };

                await AppDatabaseContext
                    .Set<UserDataModel>()
                    .AddAsync(newUser);
            }
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        return CommandResponse<int?>
        (
            content: affectedEntitiesCount,
            errorDetail: $"Было создано {affectedEntitiesCount} новых записей."
        );
    }
}