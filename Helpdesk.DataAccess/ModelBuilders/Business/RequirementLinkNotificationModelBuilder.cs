using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class RequirementLinkNotificationModelBuilder
{
    public static ModelBuilder BuildRequirementLinkNotificationModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<RequirementLinkNotificationDataModel>()
            .ToTable("requirement_link_notification", "business");

        entity
            .HasOne(l => l.Notification)
            .WithOne(n => n.RequirementLinkNotification)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }
}