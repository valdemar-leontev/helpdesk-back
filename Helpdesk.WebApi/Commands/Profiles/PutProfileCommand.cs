using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class PutProfileCommand : DataCommand
{
    public PutProfileCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileDataModel?>> PutAsync(ProfileModel profile)
    {
        profile.Id = default;

        var profileEntryEntity = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .AddAsync(new ProfileDataModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PositionId = profile.PositionId,
                UserId = profile.UserId,
            });

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<ProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была сохранена."
            );
        }

        if (profileEntryEntity.Entity.Id != default && profile.SubdivisionId is not null)
        {
            await AppDatabaseContext
                .Set<ProfileLinkSubdivisionDataModel>().AddAsync(new ProfileLinkSubdivisionDataModel
                {
                    ProfileId = profileEntryEntity.Entity.Id,
                    SubdivisionId = (int)profile.SubdivisionId
                });
        }

        affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<ProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileLinkSubdivisionDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<ProfileDataModel?>
        (
            content: profileEntryEntity.Entity
        );
    }
}