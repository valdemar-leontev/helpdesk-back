using Microsoft.EntityFrameworkCore;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class VariantModelBuilder
{
    public static ModelBuilder BuildVariantModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<VariantDataModel>()
            .ToTable("variant", "business");

        entity
            .Property(variant => variant.Description)
            .HasMaxLength(256);

        entity
            .HasOne(variant => variant.Question)
            .WithMany(question => question.Variants)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasData(new[]
            {
                new VariantDataModel { Id = 40, Description = "1201", QuestionId = 10 },
                new VariantDataModel { Id = 41, Description = "1202", QuestionId = 10 },
                new VariantDataModel { Id = 42, Description = "1203", QuestionId = 10 },
                new VariantDataModel { Id = 43, Description = "1204", QuestionId = 10 },
                new VariantDataModel { Id = 44, Description = "1205", QuestionId = 10 },
                new VariantDataModel { Id = 45, Description = "1206", QuestionId = 10 },
                new VariantDataModel { Id = 46, Description = "1207", QuestionId = 10 },
                new VariantDataModel { Id = 47, Description = "1208", QuestionId = 10 },
                new VariantDataModel { Id = 48, Description = "1209", QuestionId = 10 },
                new VariantDataModel { Id = 49, Description = "1210", QuestionId = 10 },
                new VariantDataModel { Id = 50, Description = "1211", QuestionId = 10 },
                new VariantDataModel { Id = 51, Description = "1212", QuestionId = 10 },
                new VariantDataModel { Id = 52, Description = "1213", QuestionId = 10 },
                new VariantDataModel { Id = 53, Description = "1214", QuestionId = 10 },
                new VariantDataModel { Id = 54, Description = "1215", QuestionId = 10 },
                new VariantDataModel { Id = 55, Description = "1216", QuestionId = 10 },
                new VariantDataModel { Id = 56, Description = "1217", QuestionId = 10 }
            });

        return modelBuilder;
    }
}