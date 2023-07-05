using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class GetRequirementTemplateListCommand : DataCommand
{
    public GetRequirementTemplateListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementTemplateDataModel>>> GetAsync()
    {
        var requirementTemplates = await AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .OrderByDescending(q => q.UpdateDate)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<RequirementTemplateDataModel>>
        (
            content: requirementTemplates
        );
    }
}