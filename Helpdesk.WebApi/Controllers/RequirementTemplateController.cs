using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Commands.RequirementTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/requirement-templates")]
public class RequirementTemplateController : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromServices] GetRequirementTemplateListCommand command)
    {
        var requirementTemplateResponse = await command.GetAsync();

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }

    [HttpGet("{requirementTemplateId:int}")]
    public async Task<IActionResult> GetAsync([FromServices] GetRequirementTemplateCommand command, int requirementTemplateId)
    {
        var requirementTemplateResponse = await command.GetAsync(requirementTemplateId);

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }

    [HttpDelete("{requirementTemplateId:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteRequirementTemplateCommand command, int requirementTemplateId)
    {
        var requirementTemplateResponse = await command.DeleteAsync(requirementTemplateId);

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }

    [HttpPost("{requirementTemplateId:int}/rename")]
    public async Task<IActionResult> RenameAsync([FromServices] RenameRequirementTemplateCommand command, int requirementTemplateId, string name)
    {
        var requirementTemplateResponse = await command.PostAsync(requirementTemplateId, name);

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromServices] PutRequirementTemplateCommand command)
    {
        var requirementTemplateResponse = await command.PutAsync();

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromServices] PostRequirementTemplateCommand command,
        [FromBody] RequirementTemplateDataModel requirementTemplate)
    {
        var requirementTemplateResponse = await command.PostAsync(requirementTemplate);

        return requirementTemplateResponse.Content is not null
            ? Ok(requirementTemplateResponse.Content)
            : Problem(requirementTemplateResponse.ErrorDetail);
    }
}