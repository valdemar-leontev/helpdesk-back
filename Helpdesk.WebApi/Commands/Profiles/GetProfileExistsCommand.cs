using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class GetCurrentUserProfileExistsCommand : DataCommand
{
    public GetCurrentUserProfileExistsCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<bool?>> GetAsync()
    {
        var isExistedProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .AsNoTracking()
            .AnyAsync(p => p.UserId == UserId);

        return CommandResponse<bool?>
        (
            content: isExistedProfile
        );
    }
}