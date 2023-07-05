using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class GetRequirementCommentCommand : DataCommand
{
    public GetRequirementCommentCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementCommentDataModel?>> GetAsync(int requirementStageId)
    {
        var requirementStage = await AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .Include(r => r.RequirementStageLinkRequirementComment)
            .ThenInclude(l => l!.RequirementComment)
            .FirstOrDefaultAsync(s => s.Id == requirementStageId);

        if (requirementStage is null)
        {
            return CommandResponse<RequirementCommentDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementStageDataModel))}' не была найдена."
            );
        }

        if (requirementStage.RequirementStageLinkRequirementComment!.RequirementComment is null)
        {
            return CommandResponse<RequirementCommentDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCommentDataModel))}' не была найдена."
            );
        }

        return CommandResponse<RequirementCommentDataModel?>
        (
            requirementStage.RequirementStageLinkRequirementComment!.RequirementComment
        );
    }
}