using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementLinkProfileModelBuilder
{
    public static ModelBuilder BuildRequirementLinkProfileModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementLinkProfileDataModel>()
            .ToTable("requirement_link_profile", "business");

        entity
            .HasIndex(l => new
            {
                l.RequirementId,
                l.ProfileId
            })
            .IsUnique();

        return modelBuilder;
    }
}