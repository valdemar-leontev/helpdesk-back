using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class PostProfileCommand : DataCommand
{
    public PostProfileCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileDataModel?>> PostAsync(ProfileModel profile)
    {
        var profileEntryEntity = AppDatabaseContext
            .Set<ProfileDataModel>()
            .Update(new ProfileDataModel
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PositionId = profile.PositionId,
                UserId = profile.UserId,
            });

        var currentUser = await AppDatabaseContext
            .Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Id == profile.UserId);

        if (currentUser is null)
        {
            return CommandResponse<ProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(UserDataModel))}' не была найдена."
            );
        }

        currentUser.Email = profile.Email;

        var profileLinkSubdivision = await AppDatabaseContext
            .Set<ProfileLinkSubdivisionDataModel>()
            .FirstOrDefaultAsync(l => l.ProfileId == profile.Id);

        if (profile.SubdivisionId is not null)
        {
            if (profileLinkSubdivision is not null)
            {
                profileLinkSubdivision.SubdivisionId = (int)profile.SubdivisionId;
            }
            else
            {
                await AppDatabaseContext
                    .Set<ProfileLinkSubdivisionDataModel>()
                    .AddAsync(new ProfileLinkSubdivisionDataModel
                    {
                        ProfileId = profile.Id,
                        SubdivisionId = (int)profile.SubdivisionId
                    });
            }
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<ProfileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была успешно сохранена."
            );
        }

        return CommandResponse<ProfileDataModel?>
        (
            content: profileEntryEntity.Entity
        );
    }
}