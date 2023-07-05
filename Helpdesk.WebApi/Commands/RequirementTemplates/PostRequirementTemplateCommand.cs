using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class PostRequirementTemplateCommand : DataCommand
{
    public PostRequirementTemplateCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementTemplateDataModel?>> PostAsync(RequirementTemplateDataModel requirementTemplate)
    {
        requirementTemplate.UpdateDate = DateTimeOffset.UtcNow;

        var entityEntry = AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .Update(requirementTemplate);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementTemplateDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementTemplateDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<RequirementTemplateDataModel?>
        (
            content: entityEntry.Entity
        );
    }
}