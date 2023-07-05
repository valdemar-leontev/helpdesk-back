using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Questions;

public sealed class DeleteQuestionCommand : DataCommand
{
    public DeleteQuestionCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<QuestionDataModel?>> DeleteAsync(int questionId)
    {
        var question = await AppDatabaseContext
            .Set<QuestionDataModel>()
            .FirstOrDefaultAsync(q => q.Id == questionId);

        if (question is null)
        {
            return CommandResponse<QuestionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(QuestionDataModel))}' не была найдена."
            );
        }

        var questionEntityEntry = AppDatabaseContext
            .Set<QuestionDataModel>()
            .Remove(question);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<QuestionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(QuestionDataModel))}' не была удалена."
            );
        }

        return CommandResponse<QuestionDataModel?>
        (
            content: questionEntityEntry.Entity
        );
    }
}