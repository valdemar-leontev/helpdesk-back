using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Integrations;

public sealed class GetActiveDirectoryListCommand : DataCommand
{
    public GetActiveDirectoryListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<ActiveDirectoryUserModel>>> GetAsync()
    {
        var users = await AppDatabaseContext
            .Set<UserDataModel>()
            .Where(u => u.ObjectSid != null)
            .Select(u => Mapper.Map<ActiveDirectoryUserModel>(u))
            .ToArrayAsync();

        return CommandResponse<IEnumerable<ActiveDirectoryUserModel>>
        (
            content: users
        );
    }
}