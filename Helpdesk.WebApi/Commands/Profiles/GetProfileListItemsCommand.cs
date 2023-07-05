using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class GetProfileListItemsCommand : DataCommand
{
    public GetProfileListItemsCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<ProfileListItemModel>>> GetAsync(int? subdivisionId)
    {
        var profileListItems = await AppDatabaseContext
            .Set<UserDataModel>()
            .Include(u => u.Profile!)
            .ThenInclude(p => p.ProfileLinkSubdivision)
            .ThenInclude(l => l!.Subdivision)
            .Include(u => u.Profile!)
            .ThenInclude(p => p.Position)
            .Where(u =>
                (
                    subdivisionId != null &&
                    u.Profile != null &&
                    u.Profile.ProfileLinkSubdivision != null &&
                    u.Profile.ProfileLinkSubdivision.SubdivisionId == subdivisionId
                ) ||
                subdivisionId == null)
            .ProjectTo<ProfileListItemModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<ProfileListItemModel>>
        (
            content: profileListItems
        );
    }
}