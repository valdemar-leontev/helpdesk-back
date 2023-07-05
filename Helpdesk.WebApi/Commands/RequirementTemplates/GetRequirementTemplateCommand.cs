using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.RequirementTemplates;

public sealed class GetRequirementTemplateCommand : DataCommand
{
    public GetRequirementTemplateCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<RequirementTemplateDataModel?>> GetAsync(int requirementTemplateId)
    {
        var requirementTemplate = await AppDatabaseContext
            .Set<RequirementTemplateDataModel>()
            .Include(requirementTemplate => requirementTemplate.Questions)!
            .ThenInclude(question => question.Variants)
            .FirstOrDefaultAsync(requirementTemplate => requirementTemplate.Id == requirementTemplateId);

        if (requirementTemplate is null)
        {
            return CommandResponse<RequirementTemplateDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(RequirementTemplateDataModel))}' не была найдена."
            );
        }

        if (requirementTemplate.Questions != null)
        {
            requirementTemplate.Questions = requirementTemplate.Questions.OrderBy(q => q.Id).ToList();

            foreach (var question in requirementTemplate.Questions)
            {
                if (question.Variants != null)
                {
                    question.Variants = question.Variants.OrderBy(a => a.Id).ToList();
                }
            }
        }

        return CommandResponse<RequirementTemplateDataModel?>
        (
            content: requirementTemplate
        );
    }
}