using Helpdesk.WebApi.Commands.Variants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/variants")]
public class VariantController : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> PutAsync([FromServices] PutVariantCommand command, int questionId)
    {
        var variantResponse = await command.PutAsync(questionId);

        return variantResponse.Content is not null
            ? Ok(variantResponse.Content)
            : Problem(variantResponse.ErrorDetail);
    }

    [HttpDelete("{variantId:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteVariantCommand command, int variantId)
    {
        var variantResponse = await command.DeleteAsync(variantId);

        return variantResponse.Content is not null
            ? Ok(variantResponse.Content)
            : Problem(variantResponse.ErrorDetail);
    }
}