using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PutRequirementCategoryCommand : DataCommand
{
    public PutRequirementCategoryCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementCategoryDataModel?>> PutAsync(RequirementCategoryDataModel requirementCategory)
    {
        var updatedRequirementCategory = await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .FirstOrDefaultAsync(c => c.Id == requirementCategory.Id);

        if (updatedRequirementCategory is not null)
        {
            updatedRequirementCategory.Description = requirementCategory.Description;
            updatedRequirementCategory.RequirementCategoryTypeId = requirementCategory.RequirementCategoryTypeId;
            updatedRequirementCategory.HasAgreement = requirementCategory.HasAgreement;
        }

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementCategoryDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<RequirementCategoryDataModel?>
        (
            updatedRequirementCategory
        );
    }
}