using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PostRequirementCategoryCommand : DataCommand
{
    public PostRequirementCategoryCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementCategoryDataModel?>> PostAsync(RequirementCategoryDataModel requirementCategory)
    {
        var newRequirementCategory = new RequirementCategoryDataModel
        {
            Description = requirementCategory.Description,
            RequirementCategoryTypeId = requirementCategory.RequirementCategoryTypeId,
            HasAgreement = requirementCategory.HasAgreement
        };

        await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .AddAsync(newRequirementCategory);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementCategoryDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryDataModel))}' не была создана."
            );
        }

        return CommandResponse<RequirementCategoryDataModel?>
        (
            newRequirementCategory
        );
    }
}