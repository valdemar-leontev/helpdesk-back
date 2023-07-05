using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class GetProfileIdCommand : DataCommand
{
    public GetProfileIdCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<int>> GetAsync()
    {
        var profileId = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .AsNoTracking()
            .Where(p => p.UserId == UserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        return CommandResponse
        (
            profileId
        );
    }
}