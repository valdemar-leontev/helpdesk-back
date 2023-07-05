using Helpdesk.Domain.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Admin;

public static class UserSessionModelBuilder
{
    public static ModelBuilder BuildUserSessionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<UserSessionDataModel>()
            .ToTable("user_session", "admin");

        entity
            .Property(userSession => userSession.RefreshToken)
            .HasMaxLength(96);

        entity
            .HasOne(userSession => userSession.User)
            .WithMany(user => user.UserSessions)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }
}