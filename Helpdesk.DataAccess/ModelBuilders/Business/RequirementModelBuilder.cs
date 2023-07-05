using Microsoft.EntityFrameworkCore;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementModelBuilder
{
    public static ModelBuilder BuildRequirementModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementDataModel>()
            .ToTable("requirement", "business");

        entity
            .Property(requirement => requirement.Name)
            .HasMaxLength(256);

        entity
            .HasOne(requirement => requirement.RequirementState)
            .WithMany(requirementState => requirementState.Requirements);

        entity
            .HasOne(requirement => requirement.RequirementCategory)
            .WithMany(requirementCategory => requirementCategory.Requirements);

        entity
            .HasMany(requirement => requirement.RequirementComments)
            .WithOne(requirementComments => requirementComments.Requirement)
            .OnDelete(DeleteBehavior.Cascade);

        var now = DateTimeOffset.UtcNow;

        entity
            .HasData(new RequirementDataModel
            {
                Id = 1,
                Name = "ТЗ программистам",
                RequirementTemplateId = 1,
                RequirementCategoryId = 7,
                ProfileId = 1,
                CreationDate = now,
                RequirementStateId = 1,
                OutgoingNumber = 1
            });


        return modelBuilder;
    }
}