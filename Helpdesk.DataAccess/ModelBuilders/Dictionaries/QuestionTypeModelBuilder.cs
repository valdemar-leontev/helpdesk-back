using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class QuestionTypeModelBuilder
{
    public static ModelBuilder BuildQuestionTypeModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<QuestionTypeDataModel>()
            .ToTable("question_type", "dictionaries");

        entity
            .Property(qt => qt.Description)
            .HasMaxLength(256);

        entity
            .HasData(new[]
            {
                new QuestionTypeDataModel { Id = 1, Description = "Простой текстовый вопрос" },
                new QuestionTypeDataModel { Id = 2, Description = "Вопрос с одним вариантом" },
                new QuestionTypeDataModel { Id = 3, Description = "Вопрос с множеством вариантов" },
                new QuestionTypeDataModel { Id = 4, Description = "Вопрос с выпадающим списком" },
                new QuestionTypeDataModel { Id = 5, Description = "Развернутый ответ" }
            });

        return modelBuilder;
    }
}