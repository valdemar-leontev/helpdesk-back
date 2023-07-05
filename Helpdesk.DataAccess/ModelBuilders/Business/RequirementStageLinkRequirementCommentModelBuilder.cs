using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementStageLinkRequirementCommentModelBuilder
{
    public static ModelBuilder BuildRequirementStageLinkRequirementCommentModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementStageLinkRequirementCommentDataModel>()
            .ToTable("requirement_stage_link_requirement_comment", "business");

        entity
            .HasIndex(r => new
            {
                r.RequirementStageId,
                r.RequirementCommentId
            })
            .IsUnique();

        entity
            .HasOne(l => l.RequirementComment)
            .WithOne(rs => rs.RequirementStageLinkRequirementComment)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasData(new RequirementStageLinkRequirementCommentDataModel
            {
                Id = 1,
                RequirementCommentId = 1,
                RequirementStageId = 1
            });

        return modelBuilder;
    }
}