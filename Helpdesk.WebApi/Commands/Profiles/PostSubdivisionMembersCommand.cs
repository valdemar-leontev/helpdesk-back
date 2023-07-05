using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class PostSubdivisionMembersCommand : DataCommand
{
    public PostSubdivisionMembersCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<ProfileListItemModel>>> PostAsync(ProfileListItemModel[] updatedProfileList)
    {
        foreach (var updatedProfile in updatedProfileList)
        {
            var entity = await AppDatabaseContext
                .Set<ProfileLinkSubdivisionDataModel>()
                .FirstOrDefaultAsync(l => l.ProfileId == updatedProfile.Id);

            if (updatedProfile.SubdivisionId is null)
            {
                if (entity is not null)
                {
                    AppDatabaseContext
                        .Set<ProfileLinkSubdivisionDataModel>()
                        .Remove(entity);
                }
            }
            else
            {
                if (entity is not null)
                {
                    entity.SubdivisionId = (int)updatedProfile.SubdivisionId;
                }
                else
                {
                    var newProfileLinkSubdivision = new ProfileLinkSubdivisionDataModel
                    {
                        ProfileId = updatedProfile.Id,
                        SubdivisionId = (int)updatedProfile.SubdivisionId,
                    };

                    await AppDatabaseContext
                        .Set<ProfileLinkSubdivisionDataModel>()
                        .AddAsync(newProfileLinkSubdivision);
                }
            }
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<ProfileListItemModel>>
            (
                errorDetail: $"Сущности {Description(typeof(ProfileLinkSubdivisionDataModel))} не были сохранены."
            );
        }

        return CommandResponse<IEnumerable<ProfileListItemModel>>
        (
            content: updatedProfileList
        );
    }
}