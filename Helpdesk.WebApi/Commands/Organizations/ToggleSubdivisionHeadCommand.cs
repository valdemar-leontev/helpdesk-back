using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class ToggleSubdivisionHeadCommand : DataCommand
{
    public ToggleSubdivisionHeadCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileLinkSubdivisionDataModel?>> PostAsync(int profileId)
    {
        var profileLink = await AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .FirstOrDefaultAsync(l => l.ProfileId == profileId);

        if (profileLink is null)
        {
            return CommandResponse<ProfileLinkSubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileLinkSubdivisionDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var existedHeadProfileLinks = await AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .Where(l => l.SubdivisionId == profileLink.SubdivisionId && l.IsHead == true && l.ProfileId != profileId)
            .ToListAsync();

        existedHeadProfileLinks.ForEach(l => l.IsHead = false);

        AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .UpdateRange(existedHeadProfileLinks);

        profileLink.IsHead = !profileLink.IsHead;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<ProfileLinkSubdivisionDataModel?>
            (
                errorDetail: "Руководитель подразделения не был назначен."
            );
        }

        return CommandResponse<ProfileLinkSubdivisionDataModel?>
        (
            content: profileLink
        );
    }
}