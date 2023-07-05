using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models.Enums;


namespace Helpdesk.WebApi.Commands;

public class GetTargetSubdivisionCommand : DataCommand
{
    private readonly List<int> _list = new();

    public GetTargetSubdivisionCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    private void PerformTreeTraversal(int id, IList<SubdivisionLinkSubdivisionDataModel> subdivisionLinks)
    {
        foreach (var link in subdivisionLinks)
        {
            if (link.SubdivisionId != id)
            {
                continue;
            }

            _list.Add(link.SubdivisionId);

            var parentId = link.SubdivisionParentId;

            if (parentId.HasValue)
            {
                PerformTreeTraversal(parentId.Value, subdivisionLinks);
            }
        }
    }

    public async Task<int> GetAsync(TargetSubdivisionModes mode)
    {
        var subdivisionLinks = await AppDatabaseContext
            .Set<SubdivisionLinkSubdivisionDataModel>()
            .ToListAsync();

        var profileSubdivisionId = await AppDatabaseContext
            .Set<ProfileDataModel>()
            .Include(p => p.ProfileLinkSubdivision)
            .Where(p => p.UserId == UserId && p.ProfileLinkSubdivision != null)
            .Select(p => p.ProfileLinkSubdivision!.SubdivisionId)
            .FirstOrDefaultAsync();

        PerformTreeTraversal(profileSubdivisionId, subdivisionLinks);

        return mode switch
        {
            TargetSubdivisionModes.BeforeRoot => _list[^2],
            TargetSubdivisionModes.Direct => _list[default],
            _ => _list[^2]
        };
    }
}