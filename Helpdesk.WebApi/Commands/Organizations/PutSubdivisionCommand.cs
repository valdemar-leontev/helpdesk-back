using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class PutSubdivisionCommand : DataCommand
{
    public PutSubdivisionCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<SubdivisionLinkSubdivisionDataModel?>> PutAsync(SubdivisionDataModel subdivision, int parentSubdivisionId)
    {
        subdivision.Id = default;

        var subdivisionEntityEntry = await AppDatabaseContext
            .Set<SubdivisionDataModel>()
            .AddAsync(subdivision);

        await AppDatabaseContext.SaveChangesAsync();

        if (subdivisionEntityEntry.Entity.Id == default)
        {
            return CommandResponse<SubdivisionLinkSubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(SubdivisionDataModel))}' не была создана."
            );
        }

        var subdivisionLinkEntityEntry = await AppDatabaseContext
            .Set<SubdivisionLinkSubdivisionDataModel>()
            .AddAsync(new SubdivisionLinkSubdivisionDataModel
            {
                SubdivisionId = subdivisionEntityEntry.Entity.Id,
                SubdivisionParentId = parentSubdivisionId
            });

        await AppDatabaseContext.SaveChangesAsync();

        if (subdivisionLinkEntityEntry.Entity.Id == default)
        {
            return CommandResponse<SubdivisionLinkSubdivisionDataModel?>
            (
                errorDetail: $"Ссылка типа '{Description(typeof(SubdivisionLinkSubdivisionDataModel))}' не была создана."
            );
        }

        return CommandResponse<SubdivisionLinkSubdivisionDataModel?>
        (
            content: subdivisionLinkEntityEntry.Entity
        );
    }
}