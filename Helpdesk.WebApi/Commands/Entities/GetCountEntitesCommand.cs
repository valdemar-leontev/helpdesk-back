using AutoMapper;
using System.Linq.Dynamic.Core;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands.Entities;

public sealed class GetCountEntitiesCommand : DataEntityCommand
{
    public GetCountEntitiesCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public CommandResponseModel<int?> GetCount(EntityGetRequestModel entityGetRequest)
    {
        var query = BuildGetQuery(entityGetRequest, out var entityType);

        if (query is null)
        {
            return CommandResponse<int?>
            (
                errorDetail: $"Запрос для сущности типа '{Description(entityType!)}' не был построен."
            );
        }

        var count = query.Count();

        return CommandResponse<int?>
        (
            count
        );
    }
}