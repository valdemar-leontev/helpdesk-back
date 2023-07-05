using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Commands.Profiles;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/profiles")]
public class ProfileController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromServices] GetCurrentUserProfileCommand command)
    {
        var currentUserProfileResponse = await command.GetAsync();

        return currentUserProfileResponse.Content is not null
            ? Ok(currentUserProfileResponse.Content)
            : Problem(currentUserProfileResponse.ErrorDetail);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromServices] PostProfileCommand command, [FromBody] ProfileModel profile)
    {
        var profileResponse = await command.PostAsync(profile);

        return profileResponse.Content is not null
            ? Ok(profileResponse.Content)
            : Problem(profileResponse.ErrorDetail);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromServices] PutProfileCommand command, [FromBody] ProfileModel profile)
    {
        var profileResponse = await command.PutAsync(profile);

        return profileResponse.Content is not null
            ? Ok(profileResponse.Content)
            : Problem(profileResponse.ErrorDetail);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> PostChangePasswordAsync([FromServices] PostChangePasswordCommand command, [FromBody] ChangePasswordModel changePassword)
    {
        var currentUserResponse = await command.PostAsync(changePassword);

        return currentUserResponse.Content is not null
            ? Ok(currentUserResponse.Content)
            : Problem(currentUserResponse.ErrorDetail);
    }

    [HttpGet("profile-exists")]
    public async Task<IActionResult> GetProfileExistAsync([FromServices] GetCurrentUserProfileExistsCommand command)
    {
        var currentUserProfileExistsResponse = await command.GetAsync();

        return currentUserProfileExistsResponse.Content is not null
            ? Ok(currentUserProfileExistsResponse.Content)
            : Problem(currentUserProfileExistsResponse.ErrorDetail);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromServices] GetProfileListItemsCommand command, int? subdivisionId)
    {
        var profileListItemsResponse = await command.GetAsync(subdivisionId);

        return profileListItemsResponse.Content is not null
            ? Ok(profileListItemsResponse.Content)
            : Problem(profileListItemsResponse.ErrorDetail);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetCurrentProfileAsync([FromServices] GetProfileListItemCommand command, int userId)
    {
        var profileListItemResponse = await command.GetAsync(userId);

        return profileListItemResponse.Content is not null
            ? Ok(profileListItemResponse.Content)
            : Problem(profileListItemResponse.ErrorDetail);
    }

    [HttpPost("subdivision-members")]
    public async Task<IActionResult> PostSubdivisionMembersAsync([FromServices] PostSubdivisionMembersCommand command,
        [FromBody] ProfileListItemModel[] updatedProfileList)
    {
        var profileListItemsResponse = await command.PostAsync(updatedProfileList);

        return profileListItemsResponse.Content is not null
            ? Ok(profileListItemsResponse.Content)
            : Problem(profileListItemsResponse.ErrorDetail);
    }
}