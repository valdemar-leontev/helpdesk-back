using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class RenameRequirementTemplateCommand : DataCommand
{
    public RenameRequirementTemplateCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementTemplateDataModel?>> PostAsync(int requirementTemplateId, string name)
    {
        var requirementTemplate = new RequirementTemplateDataModel
        {
            Id = requirementTemplateId,
            Name = name,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var requirementTemplateEntityEntry = AppDatabaseContext.Attach(requirementTemplate);
        requirementTemplateEntityEntry.Property(q => q.Name).IsModified = true;
        requirementTemplateEntityEntry.Property(q => q.UpdateDate).IsModified = true;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementTemplateDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<RequirementTemplateDataModel?>
        (
            content: requirementTemplateEntityEntry.Entity
        );
    }
}