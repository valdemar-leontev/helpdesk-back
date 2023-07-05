using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class DeleteRequirementCategoryCommand : DataCommand
{
    public DeleteRequirementCategoryCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementCategoryDataModel?>> DeleteAsync(int requirementCategoryId)
    {
        var deletedRequirementCategory = await AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .FirstOrDefaultAsync(c => c.Id == requirementCategoryId);

        if (deletedRequirementCategory == null)
        {
            return CommandResponse<RequirementCategoryDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var entityEntry = AppDatabaseContext
            .Set<RequirementCategoryDataModel>()
            .Remove(deletedRequirementCategory);


        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<RequirementCategoryDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementCategoryDataModel))}' не была удалена."
            );
        }

        return CommandResponse<RequirementCategoryDataModel?>
        (
            content: entityEntry.Entity
        );
    }
}