using AutoMapper;
using System.Linq.Dynamic.Core;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands.Entities;

public sealed class DeleteEntitiesCommand : DataEntityCommand
{
    public DeleteEntitiesCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<object>?>> DeleteAsync(EntityGetRequestModel entityGetRequest)
    {
        var query = BuildGetQuery(entityGetRequest, out var entityType);

        if (query is null)
        {
            return CommandResponse<IEnumerable<object>?>
            (
                errorDetail: $"Запрос для сущности типа  '{Description(entityType!)}' не был построен."
            );
        }

        var deletingEntities = await query.ToDynamicArrayAsync();

        if (!deletingEntities.Any())
        {
            return CommandResponse<IEnumerable<object>?>
            (
                errorDetail: $"Удаляемые сущности '{Description(entityType!)}' не были найдены."
            );
        }

        AppDatabaseContext.RemoveRange(deletingEntities);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<object>?>
            (
                errorDetail: $"Сущности '{Description(entityType!)}' не были удалены."
            );
        }

        return CommandResponse<IEnumerable<object>?>
        (
            deletingEntities
        );
    }
}