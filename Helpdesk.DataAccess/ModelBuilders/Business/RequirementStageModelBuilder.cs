using Microsoft.EntityFrameworkCore;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementStageModelBuilder
{
    public static ModelBuilder BuildRequirementStageModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementStageDataModel>()
            .ToTable("requirement_stage", "business");

        var now = DateTimeOffset.UtcNow;

        entity
            .HasData(new RequirementStageDataModel
            {
                Id = 1,
                RequirementId = 1,
                StateId = 1,
                ProfileId = 1,
                CreationDate = now
            });

        entity
            .HasOne(rs => rs.RequirementStageLinkRequirementComment)
            .WithOne(l => l.RequirementStage)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }
}