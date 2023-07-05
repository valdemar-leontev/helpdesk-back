using Helpdesk.WebApi.Commands.Integrations;
using Helpdesk.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/integrations")]
public class IntegrationController : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> PutUserAsync([FromServices] PutActiveDirectoryUsersCommand command,
        [FromBody] ActiveDirectoryUserModel[] activeDirectoryUsers)
    {
        var activeDirectoryUsersCountResponse = await command.PutAsync(activeDirectoryUsers);

        return activeDirectoryUsersCountResponse.Content is not null
            ? Ok(activeDirectoryUsersCountResponse.Content)
            : Problem(detail: activeDirectoryUsersCountResponse.ErrorDetail);
    }

    [HttpGet("active-directory-list")]
    public async Task<IActionResult> GetListAsync([FromServices] GetActiveDirectoryListCommand command)
    {
        var activeDirectoryUsersResponse = await command.GetAsync();

        return activeDirectoryUsersResponse.Content is not null
            ? Ok(activeDirectoryUsersResponse.Content)
            : Problem(detail: activeDirectoryUsersResponse.ErrorDetail);
    }
}