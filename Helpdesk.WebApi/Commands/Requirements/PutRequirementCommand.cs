using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PutRequirementCommand : DataCommand
{
    public PutRequirementCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementDataModel?>> PutAsync(RequirementDataModel requirement)
    {
        var now = DateTimeOffset.UtcNow;

        var requirementProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(p => p.Id == requirement.ProfileId);

        var recipientProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(p => p.UserId == UserId);

        if (requirementProfile is null || recipientProfile is null)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была найдена."
            );
        }

        var requirementCommentMessage = $"Вы создали заявку <b>{requirement.Name}</b>";
        requirement.CreationDate = now;
        requirement.RequirementStateId = (int)RequirementStates.Created;
        requirement.RequirementLinkNotification = new List<RequirementLinkNotificationDataModel>
        {
            new()
            {
                Notification = new NotificationDataModel
                {
                    Message = requirementCommentMessage,
                    IsRead = false,
                    CreationDate = now,
                    RecipientUserId = UserId
                }
            }
        };

        requirement.Stages = new List<RequirementStageDataModel>
        {
            new()
            {
                RequirementId = requirement.Id,
                StateId = (int)RequirementStates.Created,
                ProfileId = recipientProfile.Id,
                CreationDate = now
            }
        };

        requirement.RequirementLinkProfiles = new List<RequirementLinkProfileDataModel>()
        {
            new()
            {
                RequirementId = requirement.Id,
                ProfileId = requirement.ProfileId,
                IsArchive = false
            }
        };

        var entityEntry = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .AddAsync(requirement);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была создана."
            );
        }

        return CommandResponse<RequirementDataModel?>
        (
            entityEntry.Entity
        );
    }
}