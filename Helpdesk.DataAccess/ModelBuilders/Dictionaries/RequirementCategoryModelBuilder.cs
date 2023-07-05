using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class RequirementCategoryModelBuilder
{
    public static ModelBuilder BuildRequirementCategoryModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementCategoryDataModel>()
            .ToTable("requirement_category", "dictionaries");

        entity
            .HasOne(c => c.RequirementCategoryType)
            .WithMany(t => t.RequirementCategories)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasMany(c => c.RequirementCategoryLinkProfile)
            .WithOne(l => l.RequirementCategory)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .Property(rc => rc.Description)
            .HasMaxLength(256);

        entity
            .HasIndex(c => new
            {
                c.Description,
                c.RequirementCategoryTypeId
            })
            .IsUnique();

        entity
            .HasData(new[]
            {
                new RequirementCategoryDataModel { Id = 1, Description = "Отсутствие доступа к сайту", HasAgreement = false, RequirementCategoryTypeId = 1 },
                new RequirementCategoryDataModel
                    { Id = 2, Description = "Подключение к онлайн конференции", HasAgreement = false, RequirementCategoryTypeId = 1 },
                new RequirementCategoryDataModel { Id = 3, Description = "Создание онлайн конференции", HasAgreement = false, RequirementCategoryTypeId = 1 },
                new RequirementCategoryDataModel { Id = 4, Description = "Компьютер", HasAgreement = false, RequirementCategoryTypeId = 2 },
                new RequirementCategoryDataModel { Id = 5, Description = "Ноутбук", HasAgreement = false, RequirementCategoryTypeId = 2 },
                new RequirementCategoryDataModel { Id = 6, Description = "Перемещение рабочего места", HasAgreement = false, RequirementCategoryTypeId = 2 },
                new RequirementCategoryDataModel { Id = 7, Description = "Принтер (МФУ)", HasAgreement = false, RequirementCategoryTypeId = 2 },
                new RequirementCategoryDataModel
                    { Id = 8, Description = "Принтер (МФУ) / Замена картриджа", HasAgreement = false, RequirementCategoryTypeId = 2 },
                new RequirementCategoryDataModel
                    { Id = 9, Description = "Переадресация электронной почты", HasAgreement = false, RequirementCategoryTypeId = 3 },
                new RequirementCategoryDataModel { Id = 10, Description = "Создание почтового ящика", HasAgreement = false, RequirementCategoryTypeId = 3 },
                new RequirementCategoryDataModel { Id = 11, Description = "Удаление почтового ящика", HasAgreement = false, RequirementCategoryTypeId = 3 },
                new RequirementCategoryDataModel { Id = 12, Description = "Пополнение счета", HasAgreement = false, RequirementCategoryTypeId = 4 },
                new RequirementCategoryDataModel { Id = 13, Description = "Другое", HasAgreement = false, RequirementCategoryTypeId = 5 },
                new RequirementCategoryDataModel { Id = 14, Description = "Настройка почтового ящика", HasAgreement = false, RequirementCategoryTypeId = 5 },
                new RequirementCategoryDataModel { Id = 15, Description = "Настройка приложения", HasAgreement = false, RequirementCategoryTypeId = 5 },
                new RequirementCategoryDataModel { Id = 16, Description = "1С: CRM", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 17, Description = "1С: ECM", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 18, Description = "1С: ERP", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 19, Description = "Завершение сеанса 1C", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 20, Description = "Настройка", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 21, Description = "Удаление", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 22, Description = "Установка", HasAgreement = false, RequirementCategoryTypeId = 6 },
                new RequirementCategoryDataModel { Id = 23, Description = "Подключение к Wi-Fi", HasAgreement = false, RequirementCategoryTypeId = 7 },
                new RequirementCategoryDataModel { Id = 24, Description = "Настройка", HasAgreement = false, RequirementCategoryTypeId = 8 },
                new RequirementCategoryDataModel { Id = 25, Description = "Другое", HasAgreement = false, RequirementCategoryTypeId = 9 },
                new RequirementCategoryDataModel { Id = 26, Description = "Переадресация", HasAgreement = false, RequirementCategoryTypeId = 9 },
                new RequirementCategoryDataModel
                    { Id = 27, Description = "Предоставление записи разговора", HasAgreement = false, RequirementCategoryTypeId = 9 }
            });

        return modelBuilder;
    }
}