using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementCommentModelBuilder
{
    public static ModelBuilder BuildRequirementCommentModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementCommentDataModel>()
            .ToTable("requirement_comment", "business");

        entity
            .HasOne(r => r.Profile)
            .WithMany(p => p.RequirementComments)
            .HasForeignKey(r => r.SenderProfileId);

        entity
            .HasData(new RequirementCommentDataModel
            {
                Id = 1,
                SenderProfileId = 2,
                RequirementId = 1,
                Description = "That's all trash! Let's do it again!"
            });

        return modelBuilder;
    }
}