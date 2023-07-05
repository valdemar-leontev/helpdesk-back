using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class DeleteSubdivisionProfileCommand : DataCommand
{
    public DeleteSubdivisionProfileCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileLinkSubdivisionDataModel?>> DeleteAsync(int profileId)
    {
        var deletingEntity = await AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .FirstOrDefaultAsync(l => l.ProfileId == profileId);

        if (deletingEntity is null)
        {
            return new CommandResponseModel<ProfileLinkSubdivisionDataModel?>
            {
                ErrorDetail = $"Удаляемая сущность '{Description(typeof(ProfileLinkSubdivisionDataModel))}' не была найдена."
            };
        }

        var deletingEntityEntry = AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .Remove(deletingEntity);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<ProfileLinkSubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileLinkSubdivisionDataModel))}' не была удалена."
            );
        }

        return CommandResponse<ProfileLinkSubdivisionDataModel?>
        (
            content: deletingEntityEntry.Entity
        );
    }
}