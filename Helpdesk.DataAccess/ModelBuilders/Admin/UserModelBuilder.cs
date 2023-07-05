using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Admin;

public static class UserModelBuilder
{
    public static ModelBuilder BuildUserModel(this ModelBuilder modelBuilder)
    {
        // ReSharper disable once UnusedVariable
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var entity = modelBuilder
            .Entity<UserDataModel>()
            .ToTable("user", "admin");

        entity
            .Property(user => user.Name)
            .HasMaxLength(32);

        entity
            .Property(user => user.Email)
            .HasMaxLength(128);

        entity
            .Property(user => user.Password)
            .HasMaxLength(64);

        entity
            .Property(user => user.ObjectSid)
            .HasMaxLength(64);

        entity
            .HasOne(user => user.Role)
            .WithMany(role => role.Users);

        entity
            .HasIndex(u => new
            {
                u.Email,
                u.ObjectSid
            })
            .IsUnique();

        entity
            .HasData(new UserDataModel
            {
                Id = 1,
                Name = "Valdemar",
                Email = "valdemar.leontev@yandex.ru",
                Password = "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=",
                RoleId = (int)Roles.User,
                ObjectSid = null
            });

        entity
            .HasData(new UserDataModel
            {
                Id = 2,
                Name = "Bill",
                Email = "leonetx@yandex.ru",
                Password = "WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=",
                RoleId = (int)Roles.Admin,
                ObjectSid = null
            });

        return modelBuilder;
    }
}