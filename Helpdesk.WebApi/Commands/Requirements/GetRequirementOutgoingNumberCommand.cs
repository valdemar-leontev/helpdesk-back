using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class GetRequirementOutgoingNumberCommand : DataCommand
{
    public GetRequirementOutgoingNumberCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<int?>> GetAsync()
    {
        var requirementCategoryListMax = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .MaxAsync(r => (int?)r.OutgoingNumber);

        return CommandResponse(requirementCategoryListMax.HasValue ? ++requirementCategoryListMax : 1);
    }
}