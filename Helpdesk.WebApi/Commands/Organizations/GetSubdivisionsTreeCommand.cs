using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class GetSubdivisionsTreeCommand : DataCommand
{
    public GetSubdivisionsTreeCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<OrganizationTreeItemModel>>> GetAsync()
    {
        var subdivisionLinksSubdivision = await AppDatabaseContext
            .Set<SubdivisionLinkSubdivisionDataModel>()
            .Include(l => l.Subdivision)
            .ProjectTo<OrganizationTreeItemModel>(Mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToArrayAsync();

        return CommandResponse<IEnumerable<OrganizationTreeItemModel>>
        (
            content: subdivisionLinksSubdivision
        );
    }
}