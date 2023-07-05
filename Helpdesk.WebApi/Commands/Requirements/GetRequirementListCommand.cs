using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public class GetRequirementListCommand : DataCommand
{
    public GetRequirementListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<RequirementModel>>> GetAsync()
    {
        var profile = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .FirstOrDefaultAsync(p => p.UserId == UserId);

        if (profile is null)
        {
            return CommandResponse<IEnumerable<RequirementModel>>
            (
                errorDetail: $"Сущность '{Description(typeof(ProfileDataModel))}' не была найдена."
            );
        }

        await AppDatabaseContext.Set<RequirementStateDataModel>().LoadAsync();
        await AppDatabaseContext.Set<RequirementCategoryDataModel>().LoadAsync();

        var requirementsDataRecords = await AppDatabaseContext
            .Set<RequirementLinkProfileDataModel>()
            .Where(l => l.ProfileId == profile.Id)
            .Include(l => l.Requirement)
            .ThenInclude(r => r!.Stages!)
            .ThenInclude(rs => rs.Profile)
            .ThenInclude(r => r.RequirementLinkProfiles.Where(l => l.ProfileId == profile.Id))
            .Select(l => l.Requirement)
            .ToArrayAsync();

        var requirements = requirementsDataRecords
            .Select(r => Mapper.Map<RequirementModel>(r))
            .ToArray();

        return CommandResponse<IEnumerable<RequirementModel>>
        (
            requirements
        );
    }
}