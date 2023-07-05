using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Variants;

public class PutVariantCommand : DataCommand
{
    public PutVariantCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<VariantDataModel?>> PutAsync(int questionId)
    {
        var variantEntityEntry = await AppDatabaseContext
            .Set<VariantDataModel>()
            .AddAsync(new VariantDataModel
            {
                Description = Description(typeof(VariantDataModel)),
                QuestionId = questionId
            });

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<VariantDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(VariantDataModel))}' не была создана."
            );
        }

        return CommandResponse<VariantDataModel?>
        (
            content: variantEntityEntry.Entity
        );
    }
}