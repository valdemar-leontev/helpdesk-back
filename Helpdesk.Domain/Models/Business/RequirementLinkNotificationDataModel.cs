using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

public class RequirementLinkNotificationDataModel : IEntity
{
    public int Id { get; set; }

    public int NotificationId { get; set; }

    public int RequirementId { get; set; }

    public RequirementDataModel? Requirement { get; set; }

    public NotificationDataModel? Notification { get; set; }
}