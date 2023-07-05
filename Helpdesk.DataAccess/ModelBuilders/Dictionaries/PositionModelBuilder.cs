using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class PositionModelBuilder
{
    public static ModelBuilder BuildPositionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<PositionDataModel>()
            .ToTable("position", "dictionaries");

        entity
            .Property(p => p.Description)
            .HasMaxLength(256);

        entity
            .HasIndex(p => new
            {
                p.Description
            })
            .IsUnique();

        entity
            .HasData(new[]
            {
                new PositionDataModel { Id = 1, Description = "Генеральный директор" },
                new PositionDataModel { Id = 2, Description = "Руководитель отдела" },
                new PositionDataModel { Id = 3, Description = "Руководитель проекта" },
                new PositionDataModel { Id = 4, Description = "Разработчик" },
                new PositionDataModel { Id = 5, Description = "Секретарь" }
            });

        return modelBuilder;
    }
}