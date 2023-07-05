using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class SubdivisionLinkSubdivisionModelBuilder
{
    public static ModelBuilder BuildSubdivisionLinkSubdivisionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<SubdivisionLinkSubdivisionDataModel>()
            .ToTable("subdivision_link_subdivision", "business");

        entity
            .HasOne(l => l.SubdivisionParent)
            .WithMany(s => s.SubdivisionChildLinksSubdivision);

        entity
            .HasOne(l => l.Subdivision)
            .WithOne(s => s.SubdivisionParentLinkSubdivision);

        entity
            .HasIndex(l => new
            {
                l.SubdivisionId,
                l.SubdivisionParentId
            }).IsUnique();

        entity
            .HasData(new[]
            {
                new SubdivisionLinkSubdivisionDataModel {Id = 1, SubdivisionId = 1, SubdivisionParentId = null},
                new SubdivisionLinkSubdivisionDataModel {Id = 2, SubdivisionId = 2, SubdivisionParentId = 1},
            });

        return modelBuilder;
    }
}