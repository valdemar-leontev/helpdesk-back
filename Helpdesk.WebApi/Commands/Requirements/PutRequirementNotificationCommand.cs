using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PutRequirementNotificationCommand : DataCommand
{
    private readonly EmailService _emailService;

    private readonly ApplicationConfig _applicationConfig;

    public PutRequirementNotificationCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        EmailService emailService, IApplicationConfig applicationConfig)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _emailService = emailService;
        _applicationConfig = (ApplicationConfig)applicationConfig;
    }

    public async Task<CommandResponseModel<IEnumerable<NotificationDataModel?>>> PutAsync(RequirementDataModel requirement)
    {
        var mailTasks = new List<Task>();

        var requirementCategory = await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .Include(c => c.RequirementCategoryLinkProfile)
            .FirstOrDefaultAsync(c => c.Id == requirement.RequirementCategoryId);

        if (requirementCategory is null)
        {
            return CommandResponse<IEnumerable<NotificationDataModel?>>
            (
                errorDetail: $"'{Description(typeof(RequirementDataModel))}' не имеет категории."
            );
        }

        if (requirementCategory.RequirementCategoryLinkProfile is null)
        {
            return CommandResponse<IEnumerable<NotificationDataModel?>>
            (
                errorDetail: $"'{Description(typeof(RequirementCategoryDataModel))}' не имеет ответственных лиц."
            );
        }

        var requirementProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(u => u.Id == requirement.ProfileId);

        if (requirementProfile is null)
        {
            return CommandResponse<IEnumerable<NotificationDataModel?>>
            (
                errorDetail: "Отправитель заявки не был найден."
            );
        }

        var notifications = new List<NotificationDataModel>();

        var requirementCategoryProfileKeys = requirementCategory
            .RequirementCategoryLinkProfile
            .Select(l => l.ProfileId)
            .ToArray();

        var profileName = $"{requirementProfile.FirstName} {requirementProfile.LastName}";

        var responsibleProfiles = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .Include(p => p.User)
            .Where(p => requirementCategoryProfileKeys.Contains(p.Id))
            .ToArrayAsync();

        foreach (var responsibleProfile in responsibleProfiles)
        {
            var description =
                $"Вам назначена заявка с темой <b>{requirementCategory.Description} №{requirement.OutgoingNumber}</b>.<br/><br/>" +
                $"От: {requirementProfile.FirstName} {requirementProfile.LastName} <br/>";

            var notification = new NotificationDataModel()
            {
                Message = description,
                CreationDate = requirement.CreationDate,
                IsRead = false,
                RecipientUserId = responsibleProfile.UserId,
                RequirementLinkNotification = new RequirementLinkNotificationDataModel
                {
                    RequirementId = requirement.Id
                }
            };

            notifications.Add(notification);

            var message = new Message
            {
                Title = _applicationConfig.AppTitle!,
                Content =
                    $"Уважаемый (ая) {responsibleProfile.FirstName} {responsibleProfile.LastName}!<br/><br/>" +
                    $"Заявка с темой <b>{requirementCategory.Description} №{requirement.OutgoingNumber}</b> назначена вам.<br/>" +
                    $"Чтобы просмотреть данную заявку перейдите по ссылке:<br/>" +
                    $"<a>http://192.168.10.45/requirement/{requirement.RequirementTemplateId}/{requirement.Id}/?review</a>" + "<br/><br/>" +
                    $"От: {requirementProfile.FirstName} {requirementProfile.LastName}"
            };

            var mailTask = _emailService.SendMessageAsync(message, profileName, responsibleProfile.User!.Email);
            mailTasks.Add(mailTask);

            if (requirement.ProfileId != responsibleProfile.Id)
            {
                var newRequirementLinkProfile = new RequirementLinkProfileDataModel()
                {
                    RequirementId = requirement.Id,
                    ProfileId = responsibleProfile.Id,
                    IsArchive = false
                };

                requirement.RequirementLinkProfiles!.Add(newRequirementLinkProfile);
            }
        }

        await AppDatabaseContext.AddRangeAsync(notifications);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<NotificationDataModel?>>
            (
                errorDetail: $"Сущность '{Description(typeof(NotificationDataModel))}' не была создана."
            );
        }

        await Task.WhenAll(mailTasks);

        return CommandResponse<IEnumerable<NotificationDataModel?>>
        (
            notifications
        );
    }
}