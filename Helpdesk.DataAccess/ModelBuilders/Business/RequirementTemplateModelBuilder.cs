using Microsoft.EntityFrameworkCore;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementTemplateModelBuilder
{
    public static ModelBuilder BuildRequirementTemplateModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementTemplateDataModel>()
            .ToTable("requirement_template", "business");

        entity
            .Property(requirementTemplate => requirementTemplate.Name)
            .HasMaxLength(256);

        entity
            .Property(requirementTemplate => requirementTemplate.Description)
            .HasMaxLength(256);

        var now = DateTimeOffset.UtcNow;

        entity
            .HasData(new RequirementTemplateDataModel
                {
                    Id = 1,
                    Name = "Заявка по IT обеспечению",
                    Description = "Эта заявка имеет своей целью выявление и решение проблем в сфере IT обеспечения предприятия.",
                    HasRequirementCategory = true,
                    CreationDate = now,
                    UpdateDate = now
                }
            );

        return modelBuilder;
    }
}