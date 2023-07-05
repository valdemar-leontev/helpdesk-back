using AutoMapper;
using Newtonsoft.Json;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.Extensions;
using Helpdesk.Domain.Contracts;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Commands.Entities;

public class PutEntityCommand : DataEntityCommand
{
    public PutEntityCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<object?>> PutAsync(EntityPutRequestModel entityPutRequest)
    {
        var entityType = AppDatabaseContext.Model.FindModelClrEntityType(entityPutRequest.EntityTypeName);

        if (entityType is null || !typeof(IEntity<>).IsAssignableFromGenericType(entityType))
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Указан неверный сущностный тип '{entityPutRequest.EntityTypeName}'."
            );
        }

        var deserializedObject = JsonConvert.DeserializeObject(entityPutRequest.Json, entityType);

        if (deserializedObject is not IEntity entity)
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Сущность '{Description(entityType)}' не была десериализована."
            );
        }

        entity.Id = default;
        var entityEntry = await AppDatabaseContext.AddAsync(deserializedObject);
        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Сущность '{Description(entityType)}' не была сохранена."
            );
        }

        return CommandResponse<object?>
        (
            entityEntry.Entity
        );
    }
}