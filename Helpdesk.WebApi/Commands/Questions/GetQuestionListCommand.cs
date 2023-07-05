using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Questions;

public sealed class GetQuestionListCommand : DataCommand
{
    public GetQuestionListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<QuestionDataModel>>> GetAsync(int requirementTemplateId)
    {
        var questions = await AppDatabaseContext
            .Set<QuestionDataModel>()
            .Where(question => question.Id == requirementTemplateId)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<QuestionDataModel>>
        (
            content: questions
        );
    }
}