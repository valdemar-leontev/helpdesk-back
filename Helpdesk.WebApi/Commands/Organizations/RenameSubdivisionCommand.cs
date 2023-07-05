using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Organizations;

public sealed class RenameSubdivisionCommand : DataCommand
{
    public RenameSubdivisionCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<SubdivisionDataModel?>> PostAsync(int subdivisionId, string subdivisionDescription)
    {
        var updatedSubdivision = await AppDatabaseContext
            .Set<SubdivisionDataModel>()
            .FirstOrDefaultAsync(s => s.Id == subdivisionId);

        if (updatedSubdivision is null)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(SubdivisionDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        updatedSubdivision.Description = subdivisionDescription;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<SubdivisionDataModel?>
            (
                errorDetail: "Переименование не было выполнено."
            );
        }

        return CommandResponse<SubdivisionDataModel?>
        (
            content: updatedSubdivision
        );
    }
}