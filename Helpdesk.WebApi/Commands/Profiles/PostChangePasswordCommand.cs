using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Profiles;

public sealed class PostChangePasswordCommand : DataCommand
{
    public PostChangePasswordCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
    }

    public async Task<CommandResponseModel<UserDataModel?>> PostAsync(ChangePasswordModel changePassword)
    {
        var oldPasswordHash = await DataProtectionService.GetHashStringAsync(changePassword.OldPassword);
        var newPasswordHash = await DataProtectionService.GetHashStringAsync(changePassword.NewPassword);

        var currentUser = await AppDatabaseContext
            .Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Id == UserId && u.Password == oldPasswordHash);

        if (currentUser is null)
        {
            return CommandResponse<UserDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(UserDataModel))}' не была найдена.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        currentUser.Password = newPasswordHash;

        var currentUserEntryEntity = AppDatabaseContext.Entry(currentUser);
        currentUserEntryEntity.Property(user => user.Password).IsModified = true;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<UserDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(UserDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<UserDataModel?>
        (
            content: currentUserEntryEntity.Entity
        );
    }
}