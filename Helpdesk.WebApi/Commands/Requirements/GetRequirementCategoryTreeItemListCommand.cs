using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class GetRequirementCategoryTreeItemListCommand : DataCommand
{
    public GetRequirementCategoryTreeItemListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementCategoryTreeItemModel>>> GetAsync()
    {
        var requirementCategoryTreeItemList = await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .Include(r => r.RequirementCategoryType)
            .ProjectTo<RequirementCategoryTreeItemModel>(Mapper.ConfigurationProvider)
            .OrderBy(r => r.Id)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<RequirementCategoryTreeItemModel>>
        (
            content: requirementCategoryTreeItemList
        );
    }
}