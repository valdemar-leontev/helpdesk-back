using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Commands.Requirements;
using Helpdesk.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/requirements")]
public class RequirementController : ControllerBase
{
    /*[HttpPut("with-agreement")]
    public async Task<IActionResult> CreateAsync([FromBody] RequirementDataModel requirement,
        [FromServices] GetTargetSubdivisionCommand getTargetSubdivisionCommand)
    {
        var userId = HttpContext.GetUserId();
        var now = DateTimeOffset.UtcNow;
        var hasAgreement = false;
        var targetSubdivisionHead = null as ProfileLinkSubdivisionDataModel;
        var userProfile = null as ProfileDataModel;

        requirement.CreationDate = now;
        requirement.RequirementStateId = (int)RequirementStates.Created;

        if (requirement.RequirementCategoryId is not null)
        {
            hasAgreement = await _appDatabaseContext
                .Set<RequirementCategoryDataModel>()
                .Where(r => r.Id == requirement.RequirementCategoryId)
                .Select(r => r.HasAgreement)
                .SingleOrDefaultAsync();
        }

        if (hasAgreement)
        {
            userProfile = await _appDatabaseContext
                .Set<ProfileDataModel>()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            var targetSubdivisionId = await getTargetSubdivisionCommand.GetAsync(TargetSubdivisionModes.BeforeRoot);

            targetSubdivisionHead = await _appDatabaseContext
                .Set<ProfileLinkSubdivisionDataModel>()
                .Include(l => l.Profile)
                .FirstOrDefaultAsync(l => l.SubdivisionId == targetSubdivisionId && l.IsHead);

            if (userProfile is null || targetSubdivisionHead is null)
            {
                return Problem(detail: "Отсутствует профиль пользователя или нет руководителя подразделения.");
            }
        }

        requirement.Stages = new List<RequirementStageDataModel>
        {
            new()
            {
                RequirementId = requirement.Id,
                StateId = (int)RequirementStates.Created,
                UserId = userId,
                CreationDate = now
            }
        };

        var entityEntry = await _appDatabaseContext
            .Set<RequirementDataModel>()
            .AddAsync(requirement);

        var affectedEntitiesCount = await _appDatabaseContext.SaveChangesAsync();

        var newNotification = new NotificationDataModel
        {
            Message = $"Пользователь {userProfile!.FirstName} {userProfile!.LastName} отправил вам заявку на тему '{requirement.Name}' ",
            CreationDate = now,
            IsRead = false,
            RecipientUserId = targetSubdivisionHead!.Profile!.UserId,
            RequirementId = requirement.Id
        };

        await _appDatabaseContext
            .Set<NotificationDataModel>()
            .AddAsync(newNotification);

        await _appDatabaseContext.SaveChangesAsync();

        return affectedEntitiesCount > default(int)
            ? Ok(entityEntry.Entity)
            : Problem();
    }*/

    [HttpPut]
    public async Task<IActionResult> CreateAsync(
        [FromServices] PutRequirementCommand createRequirementCommand,
        [FromServices] PutRequirementNotificationCommand sendRequirementNotificationCommand,
        [FromBody] RequirementDataModel requirement
    )
    {
        var requirementResponse = await createRequirementCommand.PutAsync(requirement);

        if (requirementResponse.Content is not null)
        {
            await sendRequirementNotificationCommand.PutAsync(requirementResponse.Content);
        }

        return requirementResponse.Content is not null
            ? Ok(requirementResponse.Content)
            : Problem(requirementResponse.ErrorDetail);
    }

    [HttpGet]
    public async Task<IActionResult> GetRequirementList([FromServices] GetRequirementListCommand command)
    {
        var requirementResponse = await command.GetAsync();

        return requirementResponse.Content is not null
            ? Ok(requirementResponse.Content)
            : Problem(requirementResponse.ErrorDetail);
    }

    [HttpDelete("{requirementId:int}")]
    public async Task<IActionResult> DeleteRequirement([FromServices] DeleteRequirementCommand command, int requirementId)
    {
        var requirementResponse = await command.DeleteAsync(requirementId);

        return requirementResponse.Content is not null
            ? Ok(requirementResponse.Content)
            : Problem(requirementResponse.ErrorDetail);
    }

    [HttpGet("{requirementId:int}")]
    public async Task<IActionResult> GetRequirement([FromServices] GetRequirementCommand command, int requirementId)
    {
        var requirementResponse = await command.GetAsync(requirementId);

        return requirementResponse.Content is not null
            ? Ok(requirementResponse.Content)
            : Problem(requirementResponse.ErrorDetail);
    }

    [HttpGet("requirement-stage-list/{requirementId:int}")]
    public async Task<IActionResult> GetRequirementStageList([FromServices] GetRequirementStageListCommand command, int requirementId)
    {
        var requirementStageResponse = await command.GetAsync(requirementId);

        return requirementStageResponse.Content is not null
            ? Ok(requirementStageResponse.Content)
            : Problem(requirementStageResponse.ErrorDetail);
    }

    [HttpPost("change-state/{requirementId:int}")]
    public async Task<IActionResult> PostRequirementState(
        [FromServices] PostRequirementStageCommand command,
        int requirementId,
        [FromBody] RequirementCommentStateModel requirementCommentState)
    {
        var requirement = await command.PostAsync(requirementId, requirementCommentState.State, requirementCommentState.RequirementComment);

        return requirement.Content is not null
            ? Ok(requirement.Content)
            : Problem(requirement.ErrorDetail);
    }

    [HttpGet("requirement-category-list")]
    public async Task<IActionResult> GetRequirementCategoryList([FromServices] GetRequirementCategoryListCommand command)
    {
        var requirementCategoriesResponse = await command.GetAsync();

        return requirementCategoriesResponse.Content is not null
            ? Ok(requirementCategoriesResponse.Content)
            : Problem(requirementCategoriesResponse.ErrorDetail);
    }

    [HttpGet("requirement-category-tree-item-list")]
    public async Task<IActionResult> GetRequirementCategoryTreeItemList([FromServices] GetRequirementCategoryTreeItemListCommand command)
    {
        var requirementCategoryTreeItemsResponse = await command.GetAsync();

        return requirementCategoryTreeItemsResponse.Content is not null
            ? Ok(requirementCategoryTreeItemsResponse.Content)
            : Problem(requirementCategoryTreeItemsResponse.ErrorDetail);
    }

    [HttpGet("requirement-category-profile-list/{requirementCategoryId:int}")]
    public async Task<IActionResult> GetRequirementCategoryProfileList([FromServices] GetRequirementCategoryProfileListCommand command,
        int requirementCategoryId)
    {
        var profileListItemsResponse = await command.GetAsync(requirementCategoryId);

        return profileListItemsResponse.Content is not null
            ? Ok(profileListItemsResponse.Content)
            : Problem(profileListItemsResponse.ErrorDetail);
    }

    [HttpPost("update-requirement-category-profile-list/{requirementCategoryId:int}")]
    public async Task<IActionResult> PostRequirementCategoryProfileList([FromServices] PostRequirementCategoryProfileListCommand command,
        [FromBody] ProfileListItemModel[] profileListItems, int requirementCategoryId)
    {
        var requirementCategoryLinkProfilesResponse = await command.PostAsync(profileListItems, requirementCategoryId);

        return requirementCategoryLinkProfilesResponse.Content is not null
            ? Ok(requirementCategoryLinkProfilesResponse.Content)
            : Problem(requirementCategoryLinkProfilesResponse.ErrorDetail);
    }

    [HttpDelete("requirement-category-profile-list/{profileId:int}/{requirementCategoryId:int}")]
    public async Task<IActionResult> DeleteRequirementCategoryProfileList([FromServices] DeleteRequirementCategoryProfileCommand command, int profileId,
        int requirementCategoryId)
    {
        var requirementCategoryLinkProfileResponse = await command.DeleteAsync(profileId, requirementCategoryId);

        return requirementCategoryLinkProfileResponse.Content is not null
            ? Ok(requirementCategoryLinkProfileResponse.Content)
            : Problem(requirementCategoryLinkProfileResponse.ErrorDetail);
    }

    [HttpDelete("requirement-category/{requirementCategoryId:int}")]
    public async Task<IActionResult> GetRequirementCategoryProfileList([FromServices] DeleteRequirementCategoryCommand command, int requirementCategoryId)
    {
        var requirementCategoryResponse = await command.DeleteAsync(requirementCategoryId);

        return requirementCategoryResponse.Content is not null
            ? Ok(requirementCategoryResponse.Content)
            : Problem(requirementCategoryResponse.ErrorDetail);
    }

    [HttpPost("update-requirement-category")]
    public async Task<IActionResult> PostRequirementCategory([FromServices] PostRequirementCategoryCommand command,
        [FromBody] RequirementCategoryDataModel requirementCategory)
    {
        var requirementCategoryResponse = await command.PostAsync(requirementCategory);

        return requirementCategoryResponse.Content is not null
            ? Ok(requirementCategoryResponse.Content)
            : Problem(requirementCategoryResponse.ErrorDetail);
    }

    [HttpPut("create-requirement-category")]
    public async Task<IActionResult> PutRequirementCategory([FromServices] PutRequirementCategoryCommand command,
        [FromBody] RequirementCategoryDataModel requirementCategory)
    {
        var requirementCategoryResponse = await command.PutAsync(requirementCategory);

        return requirementCategoryResponse.Content is not null
            ? Ok(requirementCategoryResponse.Content)
            : Problem(requirementCategoryResponse.ErrorDetail);
    }

    // TODO add response after renaming on comment
    [HttpGet("requirement-comment/{requirementStageId:int}")]
    public async Task<IActionResult> GetRequirementComment([FromServices] GetRequirementCommentCommand command, int requirementStageId)
    {
        var requirementResponse = await command.GetAsync(requirementStageId);

        return requirementResponse.Content is not null
            ? Ok(requirementResponse.Content)
            : Problem(requirementResponse.ErrorDetail);
    }

    [HttpGet("requirement-outgoing-number")]
    public async Task<IActionResult> GetRequirementOutgoingNumber([FromServices] GetRequirementOutgoingNumberCommand command)
    {
        var requirementOutgoingNumberResponse = await command.GetAsync();

        return Ok(requirementOutgoingNumberResponse.Content);
    }

    [HttpPost("archive/{requirementId:int}")]
    public async Task<IActionResult> PostRequirementArchive([FromServices] PostRequirementArchiveCommand command, int requirementId)
    {
        var requirementOutgoingNumberResponse = await command.PostAsync(requirementId);

        return Ok(requirementOutgoingNumberResponse.Content);
    }

    [HttpPut("reassign-requirement/{requirementId:int}")]
    public async Task<IActionResult> PutReassignRequirement(
        [FromServices] PutReassignRequirementCommand reassignCommand,
        [FromBody] ProfileListItemModel[] reassignedProfileList,
        int requirementId)
    {
        var reassignRequirementResponse = await reassignCommand.PutAsync(reassignedProfileList, requirementId);

        return reassignRequirementResponse.Content is not null
            ? Ok(reassignRequirementResponse.Content)
            : Problem(reassignRequirementResponse.ErrorDetail);
    }
}