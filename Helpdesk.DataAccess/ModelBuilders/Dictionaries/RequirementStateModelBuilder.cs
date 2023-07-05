using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class RequirementStatesModelBuilder
{
    public static ModelBuilder BuildRequirementStateModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementStateDataModel>()
            .ToTable("requirement_state", "dictionaries");

        entity
            .Property(rs => rs.Description)
            .HasMaxLength(256);

        entity
            .HasData(
                new RequirementStateDataModel { Id = 1, Description = "Создана" },
                new RequirementStateDataModel { Id = 2, Description = "В рассмотрении" },
                new RequirementStateDataModel { Id = 3, Description = "Согласована" },
                new RequirementStateDataModel { Id = 4, Description = "В исполнении" },
                new RequirementStateDataModel { Id = 5, Description = "Отказано" },
                new RequirementStateDataModel { Id = 6, Description = "Закрыта" },
                new RequirementStateDataModel { Id = 7, Description = "Выполнена" },
                new RequirementStateDataModel { Id = 8, Description = "Переназначено" }
            );

        return modelBuilder;
    }
}