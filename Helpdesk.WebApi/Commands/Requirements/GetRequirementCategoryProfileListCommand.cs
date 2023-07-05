using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class GetRequirementCategoryProfileListCommand : DataCommand
{
    public GetRequirementCategoryProfileListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<ProfileListItemModel>>> GetAsync(int requirementCategoryId)
    {
        var profileList = await AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .Include(l => l.Profile)
            .Where(l => l.RequirementCategoryId == requirementCategoryId)
            .ProjectTo<ProfileListItemModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<ProfileListItemModel>>
        (
            content: profileList
        );
    }
}