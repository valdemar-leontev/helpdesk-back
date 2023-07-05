using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementCategoryLinkProfileModelBuilder
{
    public static ModelBuilder BuildRequirementCategoryLinkProfileModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementCategoryLinkProfileDataModel>()
            .ToTable("requirement_category_link_profile", "business");

        entity
            .HasIndex(l => new
            {
                l.RequirementCategoryId,
                l.ProfileId
            })
            .IsUnique();

        entity
            .HasData(new[]
            {
                new RequirementCategoryLinkProfileDataModel
                {
                    Id = 1, RequirementCategoryId = 1, ProfileId = 1
                },
                new RequirementCategoryLinkProfileDataModel
                {
                    Id = 2, RequirementCategoryId = 1, ProfileId = 2
                },
            });

        return modelBuilder;
    }
}