using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class GetRequirementCategoryListCommand : DataCommand
{
    public GetRequirementCategoryListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementCategoryModel>>> GetAsync()
    {
        var requirementCategoryList = await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .Include(r => r.RequirementCategoryType)
            .ProjectTo<RequirementCategoryModel>(Mapper.ConfigurationProvider)
            .OrderBy(r => r.Id)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<RequirementCategoryModel>>
        (
            requirementCategoryList
        );
    }
}