using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class RoleModelBuilder
{
    public static ModelBuilder BuildRoleModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RoleDataModel>()
            .ToTable("role", "dictionaries");

        entity
            .Property(r => r.Code)
            .HasMaxLength(32);

        entity
            .Property(r => r.Description)
            .HasMaxLength(256);

        entity
            .HasData(new[]
            {
                new RoleDataModel { Id = 1, Code = "USER", Description = "Пользователь" },
                new RoleDataModel { Id = 2, Code = "ADMIN", Description = "Администратор" },
                new RoleDataModel { Id = 3, Code = "API", Description = "Api-клиент" }
            });

        return modelBuilder;
    }
}