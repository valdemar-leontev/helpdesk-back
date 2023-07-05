using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class PostRequirementCategoryProfileListCommand : DataCommand
{
    public PostRequirementCategoryProfileListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementCategoryLinkProfileDataModel?>>> PostAsync(ProfileListItemModel[] profileList,
        int requirementCategoryId)
    {
        var originalRequirementCategoryProfileList = await AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .Where(l => l.RequirementCategoryId == requirementCategoryId)
            .ToArrayAsync();

        AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .RemoveRange(originalRequirementCategoryProfileList);

        var links = profileList.Select(p => new RequirementCategoryLinkProfileDataModel
        {
            RequirementCategoryId = requirementCategoryId,
            ProfileId = p.Id,
        });

        await AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .AddRangeAsync(links);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<RequirementCategoryLinkProfileDataModel?>>
            (
                errorDetail: $"Ссылки типа '{Description(typeof(RequirementCategoryLinkProfileDataModel))}' не были сохранены."
            );
        }

        var updatedLinks = AppDatabaseContext
            .Set<RequirementCategoryLinkProfileDataModel>()
            .Local
            .ToArray();

        return CommandResponse<IEnumerable<RequirementCategoryLinkProfileDataModel?>>
        (
            content: updatedLinks
        );
    }
}