using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class PutRequirementStageCommand : DataCommand
{
    public PutRequirementStageCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementStageDataModel>> PutAsync(int requirementId,
        string requirementComment)
    {
        var now = DateTimeOffset.UtcNow;

        var currentProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .Include(p => p.User)
            .Where(p => p.User!.Id == UserId)
            .FirstAsync();

        var newRequirementStage = await AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .AddAsync(new RequirementStageDataModel
            {
                RequirementId = requirementId,
                ProfileId = currentProfile.Id,
                CreationDate = now,
                StateId = (int)RequirementStates.Reassigned,
                RequirementStageLinkRequirementComment = new RequirementStageLinkRequirementCommentDataModel
                {
                    RequirementComment = new RequirementCommentDataModel
                    {
                        Description = requirementComment,
                        SenderProfileId = currentProfile.Id,
                        RequirementId = requirementId
                    }
                }
            });

        await AppDatabaseContext.SaveChangesAsync();

        return CommandResponse<RequirementStageDataModel>
        (
            newRequirementStage.Entity
        );
    }
}