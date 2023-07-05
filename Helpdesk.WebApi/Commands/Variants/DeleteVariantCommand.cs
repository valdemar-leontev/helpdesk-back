using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Variants;

public sealed class DeleteVariantCommand : DataCommand
{
    public DeleteVariantCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<VariantDataModel?>> DeleteAsync(int variantId)
    {
        var now = DateTimeOffset.UtcNow;

        var requirementTemplate = await AppDatabaseContext
            .Set<VariantDataModel>()
            .Include(variant => variant.Question)
            .Where(variant => variant.Id == variantId)
            .Select(variant => variant.Question!.RequirementTemplate)
            .FirstOrDefaultAsync();

        if (requirementTemplate is null)
        {
            return CommandResponse<VariantDataModel?>(
                errorDetail: $"Сущность '{Description(typeof(RequirementTemplateDataModel))}' не была найдена."
            );
        }

        var variant = await AppDatabaseContext
            .Set<VariantDataModel>()
            .FirstOrDefaultAsync(v => v.Id == variantId);

        if (variant is null)
        {
            return CommandResponse<VariantDataModel?>(
                errorDetail: $"Сущность '{Description(typeof(VariantDataModel))}' не была найдена."
            );
        }

        AppDatabaseContext
            .Set<VariantDataModel>()
            .Remove(variant);

        requirementTemplate.UpdateDate = now;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<VariantDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(VariantDataModel))}' не была удалена."
            );
        }

        return CommandResponse<VariantDataModel?>
        (
            content: variant
        );
    }
}