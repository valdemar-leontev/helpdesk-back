using Microsoft.EntityFrameworkCore;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class QuestionModelBuilder
{
    public static ModelBuilder BuildQuestionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<QuestionDataModel>()
            .ToTable("question", "business");

        entity
            .Property(question => question.Description)
            .HasMaxLength(256);

        entity
            .HasOne(question => question.RequirementTemplate)
            .WithMany(requirementTemplate => requirementTemplate.Questions);

        entity
            .HasOne(question => question.QuestionType)
            .WithMany(questionType => questionType.Questions);

        entity
            .HasData(new[]
            {
                new QuestionDataModel { Id = 10, Description = "Номер офиса: ", IsRequired = true, QuestionTypeId = 4, RequirementTemplateId = 1 },
                new QuestionDataModel { Id = 11, Description = "Ваша проблема: ", IsRequired = true, QuestionTypeId = 5, RequirementTemplateId = 1 }
            });

        entity
            .HasOne(question => question.RequirementTemplate)
            .WithMany(requirementTemplate => requirementTemplate.Questions)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }
}