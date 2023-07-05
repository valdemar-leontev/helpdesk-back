using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementLinkFileModelBuilder
{
    public static ModelBuilder BuildRequirementLinkFileModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementLinkFileDataModel>()
            .ToTable("requirement_link_file", "business");

        entity
            .HasIndex(l => new
            {
                l.RequirementId,
                l.FileId
            })
            .IsUnique();

        return modelBuilder;
    }
}