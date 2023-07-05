using AutoMapper;
using System.Linq.Dynamic.Core;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands.Entities;

public sealed class GetEntitiesListCommand : DataEntityCommand
{
    public GetEntitiesListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<IEnumerable<object>?>> GetListAsync(EntityGetRequestModel entityGetRequest)
    {
        var query = BuildGetQuery(entityGetRequest, out var entityType);

        if (query is null)
        {
            return CommandResponse<IEnumerable<object>?>
            (
                errorDetail: $"Запрос для сущности типа '{Description(entityType!)}' не был построен."
            );
        }

        var entities = await query.ToDynamicArrayAsync();

        return CommandResponse<IEnumerable<object>?>
        (
            entities
        );
    }
}