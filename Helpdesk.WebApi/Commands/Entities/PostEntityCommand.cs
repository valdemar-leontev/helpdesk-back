using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.Extensions;
using Helpdesk.Domain.Contracts;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Models.Abstracts;
using Newtonsoft.Json;

namespace Helpdesk.WebApi.Commands.Entities;

public class PostEntityCommand : DataEntityCommand
{
    public PostEntityCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<object?>> PostAsync(EntityPostRequestModel entityPostRequest)
    {
        var entityType = AppDatabaseContext.Model.FindModelClrEntityType(entityPostRequest.EntityTypeName);

        if (entityType is null || !typeof(IEntity<>).IsAssignableFromGenericType(entityType))
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Указан неверный сущностный тип '{entityPostRequest.EntityTypeName}'."
            );
        }

        var deserializedObject = JsonConvert.DeserializeObject(entityPostRequest.Json, entityType);

        if (deserializedObject is not IEntity entity)
        {
            return CommandResponse<object?>
            (
                errorDetail: $"Сущность '{Description(entityType)}' не была десериализована."
            );
        }

        var entityEntry = AppDatabaseContext.Attach(entity);

        foreach (var updatedPropertyName in entityPostRequest.UpdatedProperties)
        {
            var property = entityType.GetProperty(updatedPropertyName);

            if (property is null)
            {
                continue;
            }

            entityEntry
                .Property(updatedPropertyName)
                .IsModified = true;
        }

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