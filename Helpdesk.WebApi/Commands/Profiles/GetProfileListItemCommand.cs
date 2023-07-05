using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class GetProfileListItemCommand : DataCommand
{
    public GetProfileListItemCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileListItemModel?>> GetAsync(int userId)
    {
        var currentUserProfile = await AppDatabaseContext
            .Set<UserDataModel>()
            .Include(u => u.Profile!)
            .ThenInclude(p => p.ProfileLinkSubdivision)
            .ThenInclude(l => l!.Subdivision)
            .Include(u => u.Profile!)
            .ThenInclude(p => p.Position)
            .ProjectTo<ProfileListItemModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (currentUserProfile is null)
        {
            return CommandResponse<ProfileListItemModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return CommandResponse<ProfileListItemModel?>
        (
            content: currentUserProfile
        );
    }
}