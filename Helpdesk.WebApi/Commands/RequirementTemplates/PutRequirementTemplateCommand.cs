using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class PutRequirementTemplateCommand : DataCommand
{
    public PutRequirementTemplateCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementTemplateDataModel?>> PutAsync()
    {
        var now = DateTimeOffset.UtcNow;
        var requirementTemplate = new RequirementTemplateDataModel
        {
            Name = "Новый шаблон заявки",
            CreationDate = now,
            UpdateDate = now,
            Questions = new[]
            {
                new QuestionDataModel
                {
                    Description = string.Empty,
                    QuestionTypeId = (int)QuestionTypes.SingleSelect,
                    Variants = new[]
                    {
                        new VariantDataModel
                        {
                            Description = string.Empty,
                        }
                    }
                }
            }
        };

        var requirementTemplateEntityEntry = await AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .AddAsync(requirementTemplate);

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
            content: requirementTemplateEntityEntry.Entity
        );
    }
}