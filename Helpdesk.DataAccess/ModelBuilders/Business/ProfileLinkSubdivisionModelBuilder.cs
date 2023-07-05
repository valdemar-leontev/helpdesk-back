using Helpdesk.Domain.Models.Business;

using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class ProfileLinkSubdivisionModelBuilder
{
    public static ModelBuilder BuildProfileLinkSubdivisionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<ProfileLinkSubdivisionDataModel>()
            .ToTable("profile_link_subdivision", "business");

        entity
            .HasIndex(l => new
            {
                l.ProfileId,
                l.SubdivisionId
            })
            .IsUnique();

        entity.HasData(new[]
            {
                new ProfileLinkSubdivisionDataModel {Id = 1, ProfileId = 2, SubdivisionId = 1, IsHead = true},
            });

        return modelBuilder;
    }
}