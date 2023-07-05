using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Commands.Profiles;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class DeleteRequirementCommand : DataCommand
{
    private readonly GetProfileIdCommand _getProfileIdCommand;

    public DeleteRequirementCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        GetProfileIdCommand getProfileIdCommand)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _getProfileIdCommand = getProfileIdCommand;
    }

    public async Task<CommandResponseModel<RequirementDataModel?>> DeleteAsync(int requirementId)
    {
        var requirement = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .FirstOrDefaultAsync(r => r.Id == requirementId);

        if (requirement is null)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementDataModel))}' не была найдена."
            );
        }

        var requirementCreatorProfileIdResponse = await _getProfileIdCommand.GetAsync();
        var requirementCreatorProfileId = requirementCreatorProfileIdResponse.Content;

        if (requirement.ProfileId != requirementCreatorProfileId)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Допустимо удаление только собственной сущности типа '{Description(typeof(RequirementDataModel))}'."
            );
        }

        var requirementStages = await AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .Include(s => s.RequirementStageLinkRequirementComment)
            .Where(s => s.RequirementId == requirement.Id)
            .ToArrayAsync();

        var requirementComments = await AppDatabaseContext
            .Set<RequirementCommentDataModel>()
            .Where(r => r.RequirementId == requirement.Id)
            .ToArrayAsync();

        AppDatabaseContext
            .Set<RequirementStageLinkRequirementCommentDataModel>()
            .RemoveRange(
                requirementStages
                    .Where(s => s.RequirementStageLinkRequirementComment is not null)
                    .Select(s => s.RequirementStageLinkRequirementComment)!
            );

        AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .RemoveRange(requirementStages);

        AppDatabaseContext
            .Set<RequirementCommentDataModel>()
            .RemoveRange(requirementComments);

        var requirementEntityEntry = AppDatabaseContext
            .Set<RequirementDataModel>()
            .Remove(requirement);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementDataModel))}' не была удалена."
            );
        }

        return CommandResponse<RequirementDataModel?>
        (
            requirementEntityEntry.Entity
        );
    }
}