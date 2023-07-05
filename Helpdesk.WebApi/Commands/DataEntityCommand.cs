using AutoMapper;
using System.Linq.Dynamic.Core;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.Extensions;
using Helpdesk.Domain.Contracts;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands;

public abstract class DataEntityCommand : DataCommand
{
    protected DataEntityCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    protected IQueryable? BuildGetQuery(EntityGetRequestModel entityGetRequest, out Type? entityType)
    {
        entityType = AppDatabaseContext.Model.FindModelClrEntityType(entityGetRequest.EntityTypeName);

        if (entityType is null || !typeof(IEntity<>).IsAssignableFromGenericType(entityType))
        {
            return null;
        }

        var query = AppDatabaseContext.Set(entityType);

        if (query is null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(entityGetRequest.Includes))
        {
            query = query.Include(entityType, entityGetRequest.Includes);
        }

        if (!string.IsNullOrEmpty(entityGetRequest.Filter))
        {
            query = query.Where(entityGetRequest.Filter);
        }

        return query;
    }
}