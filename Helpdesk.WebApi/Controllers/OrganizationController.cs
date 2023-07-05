using Microsoft.AspNetCore.Mvc;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Commands.Organizations;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Route("/organizations")]
public class OrganizationController : ControllerBase
{
    [HttpGet("tree")]
    public async Task<IActionResult> GetSubdivisionsTreeAsync([FromServices] GetSubdivisionsTreeCommand command)
    {
        var organizationTreeItemsResponse = await command.GetAsync();

        return organizationTreeItemsResponse.Content is not null
            ? Ok(organizationTreeItemsResponse.Content)
            : Problem(organizationTreeItemsResponse.ErrorDetail);
    }

    [HttpPut("{parentSubdivisionId:int}")]
    public async Task<IActionResult> PutSubdivisionAsync([FromServices] PutSubdivisionCommand command, SubdivisionDataModel subdivision,
        int parentSubdivisionId)
    {
        var subdivisionLinkSubdivisionResponse = await command.PutAsync(subdivision, parentSubdivisionId);

        return subdivisionLinkSubdivisionResponse.Content is not null
            ? Ok(subdivisionLinkSubdivisionResponse.Content)
            : Problem(subdivisionLinkSubdivisionResponse.ErrorDetail);
    }

    [HttpDelete("{subdivisionId:int}")]
    public async Task<IActionResult> DeleteSubdivisionAsync([FromServices] DeleteSubdivisionCommand command, int subdivisionId)
    {
        var subdivisionResponse = await command.DeleteAsync(subdivisionId);

        return subdivisionResponse.Content is not null
            ? Ok(subdivisionResponse.Content)
            : Problem(subdivisionResponse.ErrorDetail);
    }

    [HttpPost("{subdivisionId:int}/rename")]
    public async Task<IActionResult> RenameSubdivisionAsync([FromServices] RenameSubdivisionCommand command, int subdivisionId, string subdivisionDescription)
    {
        var subdivisionResponse = await command.PostAsync(subdivisionId, subdivisionDescription);

        return subdivisionResponse.Content is not null
            ? Ok(subdivisionResponse.Content)
            : Problem(subdivisionResponse.ErrorDetail);
    }

    [HttpDelete("delete-from-subdivision/{profileId:int}")]
    public async Task<IActionResult> DeleteSubdivisionProfileAsync([FromServices] DeleteSubdivisionProfileCommand command, int profileId)
    {
        var profileLinkSubdivisionResponse = await command.DeleteAsync(profileId);

        return profileLinkSubdivisionResponse.Content is not null
            ? Ok(profileLinkSubdivisionResponse.Content)
            : Problem(profileLinkSubdivisionResponse.ErrorDetail);
    }

    [HttpPost("assign-subdivision-head/{profileId:int}")]
    public async Task<IActionResult> ToggleSubdivisionHeadAsync([FromServices] ToggleSubdivisionHeadCommand command, int profileId)
    {
        var profileLinkSubdivisionResponse = await command.PostAsync(profileId);

        return profileLinkSubdivisionResponse.Content is not null
            ? Ok(profileLinkSubdivisionResponse.Content)
            : Problem(profileLinkSubdivisionResponse.ErrorDetail);
    }
}