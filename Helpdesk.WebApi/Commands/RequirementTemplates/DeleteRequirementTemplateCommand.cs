using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class DeleteRequirementTemplateCommand : DataCommand
{
    public DeleteRequirementTemplateCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementTemplateDataModel?>> DeleteAsync(int requirementTemplateId)
    {
        var requirementTemplate = await AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .FirstOrDefaultAsync(r => r.Id == requirementTemplateId);

        if (requirementTemplate is null)
        {
            return CommandResponse<RequirementTemplateDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementTemplateDataModel))}' не была найдена."
            );
        }

        var requirementTemplateEntityEntry = AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .Remove(requirementTemplate);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementTemplateDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementTemplateDataModel))}' не была удалена."
            );
        }

        return CommandResponse<RequirementTemplateDataModel?>
        (
            content: requirementTemplateEntityEntry.Entity
        );
    }
}