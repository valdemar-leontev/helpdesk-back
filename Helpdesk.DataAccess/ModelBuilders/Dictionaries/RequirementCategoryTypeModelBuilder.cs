using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class RequirementCategoryTypeModelBuilder
{
    public static ModelBuilder BuildRequirementCategoryTypeModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementCategoryTypeDataModel>()
            .ToTable("requirement_category_type", "dictionaries");

        entity
            .Property(rct => rct.Description)
            .HasMaxLength(256);

        entity
            .HasIndex(t => new
            {
                t.Description
            })
            .IsUnique();

        entity
            .HasData(new[]
            {
                new RequirementCategoryTypeDataModel { Id = 1, Description = "Интернет" },
                new RequirementCategoryTypeDataModel { Id = 2, Description = "Компьютерная техника" },
                new RequirementCategoryTypeDataModel { Id = 3, Description = "Корпоративная почта" },
                new RequirementCategoryTypeDataModel { Id = 4, Description = "Мобильная связь" },
                new RequirementCategoryTypeDataModel { Id = 5, Description = "Мобильные устройства" },
                new RequirementCategoryTypeDataModel { Id = 6, Description = "Программное обеспечение" },
                new RequirementCategoryTypeDataModel { Id = 7, Description = "Сетевое оборудование" },
                new RequirementCategoryTypeDataModel { Id = 8, Description = "Электронная подпись" },
                new RequirementCategoryTypeDataModel { Id = 9, Description = "IP-Телефония" }
            });

        return modelBuilder;
    }
}