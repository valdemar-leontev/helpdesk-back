using AutoMapper;
using System.Linq.Dynamic.Core;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands.Entities;

public sealed class GetEntityCommand : DataEntityCommand
{
    public GetEntityCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public CommandResponseModel<object?> Get(EntityGetRequestModel entityGetRequest)
    {
        var query = BuildGetQuery(entityGetRequest, out var entityType);

        if (query is null)
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Запрос для сущности типа '{Description(entityType!)}' не был построен."
            );
        }

        var entity = query.FirstOrDefault();

        return CommandResponse<object?>
        (
            content: entity
        );
    }
}