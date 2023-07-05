using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PostRequirementStageCommand : DataCommand
{
    private readonly ApplicationConfig _applicationConfig;

    private readonly EmailService _emailService;

    public PostRequirementStageCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        EmailService emailService, IApplicationConfig applicationConfig)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _emailService = emailService;
        _applicationConfig = (ApplicationConfig)applicationConfig;
    }

    // TODO: need to refactor
    public async Task<CommandResponseModel<RequirementDataModel?>> PostAsync(int requirementId, RequirementStates state,
        RequirementCommentDataModel? newRequirementComment)
    {
        var now = DateTimeOffset.UtcNow;

        var currentRequirement = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .Include(r => r.RequirementCategory)
            .Include(r => r.Profile)
            .ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(r => r.Id == requirementId);

        if (currentRequirement is null)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementDataModel))}' не была найдена."
            );
        }

        var currentProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(p => p.UserId == UserId);

        if (currentProfile is null)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была найдена."
            );
        }

        // Change requirement state
        currentRequirement.RequirementStateId = (int)state;

        // Creating new requirement stage
        var requirementStageEntityEntry = await AppDatabaseContext
            .Set<RequirementStageDataModel>()
            .AddAsync(new RequirementStageDataModel()
            {
                RequirementId = requirementId,
                StateId = (int)state,
                ProfileId = currentProfile.Id,
                CreationDate = now
            });

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementStageDataModel))}' не была создана."
            );
        }

        // Creating notification about change requirement state

        var senderProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(p => p.UserId == UserId);

        if (senderProfile is not null)
        {
            var message =
                $"Заявка <b>{currentRequirement.Name}</b> " +
                $"переведена в состояние <b>{state.GetDescription()}</b> <br/> <br/> " +
                $"От: <b>{senderProfile.FirstName} {senderProfile.LastName}</b>";

            if (newRequirementComment is not null)
            {
                newRequirementComment.SenderProfileId = senderProfile.Id;
                newRequirementComment.RequirementId = requirementId;
                newRequirementComment.RequirementStageLinkRequirementComment =
                    new RequirementStageLinkRequirementCommentDataModel
                    {
                        RequirementCommentId = default,
                        RequirementStageId = requirementStageEntityEntry.Entity.Id
                    };
            }

            if (new[] { (int)RequirementStates.InExecution, (int)RequirementStates.Rejected }.Contains(currentRequirement.RequirementStateId))
            {
                var notificationList = await AppDatabaseContext
                    .Set<RequirementLinkNotificationDataModel>()
                    .Include(l => l.Notification)
                    .Where(l => l.RequirementId == requirementId && l.Notification!.RecipientUserId == UserId)
                    .Select(l => l.Notification)
                    .ToArrayAsync();

                foreach (var notification in notificationList)
                {
                    notification!.IsRead = true;
                }
            }

            var newRequirementStageChangeNotification = new NotificationDataModel
            {
                Message = message,
                CreationDate = now,
                IsRead = false,
                RecipientUserId = currentRequirement.Profile!.UserId,
                RequirementLinkNotification = new RequirementLinkNotificationDataModel
                {
                    RequirementId = requirementId
                }
            };

            await AppDatabaseContext
                .Set<NotificationDataModel>()
                .AddAsync(newRequirementStageChangeNotification);

            if (newRequirementComment is not null)
            {
                await AppDatabaseContext
                    .Set<RequirementCommentDataModel>()
                    .AddAsync(newRequirementComment);
            }

            affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

            if (affectedEntitiesCount == default)
            {
                return CommandResponse<RequirementDataModel?>
                (
                    errorDetail: $"Сущность '{Description(typeof(RequirementStageDataModel))}' не была создана."
                );
            }

            var emailMessage = new Message
            {
                Title = _applicationConfig.AppTitle!,
                Content = message
            };

            await _emailService.SendMessageAsync(emailMessage, $"{currentProfile.FirstName} {currentProfile.LastName}", currentRequirement.Profile.User!.Email);
        }

        return CommandResponse<RequirementDataModel?>
        (
            currentRequirement
        );
    }
}