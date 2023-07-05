using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class DeleteSubdivisionCommand : DataCommand
{
    public DeleteSubdivisionCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<SubdivisionDataModel?>> DeleteAsync(int subdivisionId)
    {
        var subdivisionLink = await AppDatabaseContext
            .Set<SubdivisionLinkSubdivisionDataModel>()
            .FirstOrDefaultAsync(l => l.SubdivisionId == subdivisionId);

        if (subdivisionLink is not null && subdivisionLink.SubdivisionParentId is null)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: $"Корневое {Description(typeof(SubdivisionDataModel)).ToLower()} не может быть удалено."
            );
        }

        var subdivisionChildren = await AppDatabaseContext
            .Set<SubdivisionLinkSubdivisionDataModel>()
            .FirstOrDefaultAsync(l => l.SubdivisionParentId == subdivisionId);

        if (subdivisionChildren is not null)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(SubdivisionDataModel))}' имеет дочерние элементы и не может быть удалено."
            );
        }

        var deletingSubdivisionEntity = await AppDatabaseContext
            .Set<SubdivisionDataModel>()
            .FirstOrDefaultAsync(s => s.Id == subdivisionId);

        if (deletingSubdivisionEntity is null)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(SubdivisionDataModel))}' не найдена."
            );
        }


        AppDatabaseContext
            .Set<SubdivisionDataModel>()
            .Remove(deletingSubdivisionEntity);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(SubdivisionDataModel))}' не была удалена."
            );
        }

        return CommandResponse<SubdivisionDataModel?>
        (
            deletingSubdivisionEntity
        );
    }
}