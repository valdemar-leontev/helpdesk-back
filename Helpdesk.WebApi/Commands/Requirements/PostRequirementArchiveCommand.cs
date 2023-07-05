using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Commands.Profiles;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class PostRequirementArchiveCommand : DataCommand
{
    private readonly GetProfileIdCommand _getProfileIdCommand;

    public PostRequirementArchiveCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        GetProfileIdCommand getProfileIdCommand)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _getProfileIdCommand = getProfileIdCommand;
    }

    public async Task<CommandResponseModel<RequirementLinkProfileDataModel?>> PostAsync(int requirementId)
    {
        var profileIdResponse = await _getProfileIdCommand.GetAsync();
        var profileId = profileIdResponse.Content;

        var requirementLinkProfile = await AppDatabaseContext
            .Set<RequirementLinkProfileDataModel>()
            .FirstOrDefaultAsync(l => l.RequirementId == requirementId && l.ProfileId == profileId);

        if (requirementLinkProfile is null)
        {
            requirementLinkProfile = new RequirementLinkProfileDataModel
            {
                RequirementId = requirementId,
                ProfileId = profileId,
                IsArchive = true
            };

            await AppDatabaseContext
                .Set<RequirementLinkProfileDataModel>()
                .AddAsync(requirementLinkProfile);
        }
        else
        {
            requirementLinkProfile.IsArchive = !requirementLinkProfile.IsArchive;
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementLinkProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementLinkProfileDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<RequirementLinkProfileDataModel?>
        (
            requirementLinkProfile
        );
    }
}