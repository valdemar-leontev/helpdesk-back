using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class DeleteRequirementCategoryProfileCommand : DataCommand
{
    public DeleteRequirementCategoryProfileCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementCategoryLinkProfileDataModel?>> DeleteAsync(int profileId, int requirementCategoryId)
    {
        var deletedProfile = await AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .FirstOrDefaultAsync(l => l.ProfileId == profileId && l.RequirementCategoryId == requirementCategoryId);

        if (deletedProfile is null)
        {
            return CommandResponse<RequirementCategoryLinkProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryLinkProfileDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .Remove(deletedProfile);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementCategoryLinkProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryLinkProfileDataModel))}' не была удалена."
            );
        }

        return CommandResponse<RequirementCategoryLinkProfileDataModel?>
        (
            content: deletedProfile
        );
    }
}