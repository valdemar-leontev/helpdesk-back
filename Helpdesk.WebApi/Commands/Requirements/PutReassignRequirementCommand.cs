using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.Extensions;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class PutReassignRequirementCommand : DataCommand
{
    private readonly EmailService _emailService;

    public PutReassignRequirementCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        EmailService emailService)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _emailService = emailService;
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementLinkProfileDataModel>>> PutAsync(
        ProfileListItemModel[] reassignedProfileList,
        int requirementId)
    {
        var now = DateTimeOffset.UtcNow;
        var mailTasks = new List<Task>();

        var currentRequirement = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .Include(r => r.Profile)
            .FirstOrDefaultAsync(r => r.Id == requirementId);

        if (currentRequirement is null)
        {
            return CommandResponse<IEnumerable<RequirementLinkProfileDataModel>>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementDataModel))}' не была найдена."
            );
        }

        var currentProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .Include(p => p.User)
            .Where(p => p.User!.Id == UserId)
            .FirstAsync();

        var requirementLinkProfileStorage = new List<RequirementLinkProfileDataModel>();

        foreach (var reassignedProfile in reassignedProfileList)
        {
            var newRequirementProfileLink = await AppDatabaseContext
                .Set<RequirementLinkProfileDataModel>()
                .AddAsync(new RequirementLinkProfileDataModel()
                {
                    RequirementId = requirementId,
                    ProfileId = reassignedProfile.Id,
                    IsArchive = false
                });

            await AppDatabaseContext
                .Set<NotificationDataModel>()
                .AddAsync(new NotificationDataModel
                {
                    CreationDate = now,
                    IsDeleted = false,
                    IsRead = false,
                    RecipientUserId = reassignedProfile.UserId,
                    Message = $"Вам переназначена заявка {currentRequirement}." +
                              $"<br/> <br/>" +
                              $"От: {currentProfile.FirstName} {currentProfile.LastName}",
                    RequirementLinkNotification = new RequirementLinkNotificationDataModel
                    {
                        RequirementId = requirementId
                    }
                });

            var message = new Message
            {
                Title = "Переназначение заявки",
                Content = $"Уважаемый (ая) {reassignedProfile.FirstName} {reassignedProfile.LastName}!<br/><br/>" +
                          $"Вам переназначена следующая заявка: <br/>" +
                          $"Имя: {currentRequirement.Name}, " +
                          $"Создатель: {currentRequirement.Profile!.FirstName} {currentRequirement.Profile!.FirstName}" +
                          $"Дата: {currentRequirement.CreationDate}" +
                          $"Чтобы просмотреть данную заявку перейдите по ссылке:<br/>" +
                          $"<a>http://192.168.10.45/requirement/{currentRequirement.RequirementTemplateId}/{currentRequirement.Id}/?review</a>" +
                          $"<br/> <br/>" +
                          $"От: {currentProfile.FirstName} {currentProfile.LastName}"
            };

            var mailTask = _emailService.SendMessageAsync(message, reassignedProfile.FirstName!, reassignedProfile.Email);
            mailTasks.Add(mailTask);

            requirementLinkProfileStorage.Add(newRequirementProfileLink.Entity);
        }

        await AppDatabaseContext.SaveChangesAsync();

        await Task.WhenAll(mailTasks);

        return CommandResponse<IEnumerable<RequirementLinkProfileDataModel>>
        (
            requirementLinkProfileStorage
        );
    }
}