using Helpdesk.WebApi.Commands.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/questions")]
public class QuestionController : ControllerBase
{
    [HttpGet("{requirementTemplateId:int}")]
    public async Task<IActionResult> GetQuestionsAsync([FromServices] GetQuestionListCommand command, int requirementTemplateId)
    {
        var questionsResponse = await command.GetAsync(requirementTemplateId);

        return questionsResponse.Content is not null
            ? Ok(questionsResponse.Content)
            : Problem(questionsResponse.ErrorDetail);
    }

    [HttpDelete("{questionId:int}")]
    public async Task<IActionResult> DeleteQuestion([FromServices] DeleteQuestionCommand command, int questionId)
    {
        var questionResponse = await command.DeleteAsync(questionId);

        return questionResponse.Content is not null
            ? Ok(questionResponse.Content)
            : Problem(questionResponse.ErrorDetail);
    }
}