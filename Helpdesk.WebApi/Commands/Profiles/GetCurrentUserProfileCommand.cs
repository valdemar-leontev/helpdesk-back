using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class GetCurrentUserProfileCommand : DataCommand
{
    public GetCurrentUserProfileCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<ProfileModel?>> GetAsync()
    {
        var currentUserProfile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .Include(profile => profile.User)
            .Include(profile => profile.Position)
            .Include(profile => profile.ProfileLinkSubdivision)
            .ThenInclude(l => l!.Subdivision)
            .ProjectTo<ProfileModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(profile => profile.UserId == UserId);

        if (currentUserProfile is null)
        {
            return CommandResponse<ProfileModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return CommandResponse<ProfileModel?>
        (
            content: currentUserProfile
        );
    }
}