using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class NotificationModelBuilder
{
    public static ModelBuilder BuildNotificationModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<NotificationDataModel>()
            .ToTable("notification", "business");

        entity
            .Property(notification => notification.Message)
            .HasMaxLength(256);

        var now = DateTimeOffset.UtcNow;

        entity
            .HasData(new[]
            {
                new NotificationDataModel
                {
                    Id = 1,
                    RecipientUserId = 2,
                    Message = "Пользователь Vladimir отправил вам на согласование заявку на тему 'ТЗ программистам'",
                    CreationDate = now,
                    IsRead = false,
                    IsDeleted = false
                }
            });

        return modelBuilder;
    }
}