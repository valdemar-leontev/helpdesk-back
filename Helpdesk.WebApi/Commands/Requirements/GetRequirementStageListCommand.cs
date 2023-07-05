using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class GetRequirementStageListCommand : DataCommand
{
    public GetRequirementStageListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementStageModel>>> GetAsync(int requirementId)
    {
        var requirementStageList = await AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .Include(r => r.Profile)
            .Include(r => r.State)
            .Include(r => r.RequirementStageLinkRequirementComment)
            .Where(r => r.RequirementId == requirementId)
            .OrderByDescending(r => r.CreationDate)
            .ProjectTo<RequirementStageModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<RequirementStageModel>>
        (
            requirementStageList
        );
    }
}