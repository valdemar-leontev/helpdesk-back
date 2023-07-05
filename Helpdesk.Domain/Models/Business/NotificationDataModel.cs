using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Admin;

namespace Helpdesk.Domain.Models.Business;

[Description("Уведомление")]
public class NotificationDataModel : IEntity
{
    public int Id { get; set; }

    public int RecipientUserId { get; set; }

    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public bool IsDeleted { get; set; }

    public UserDataModel? RecipientUser { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public RequirementLinkNotificationDataModel? RequirementLinkNotification { get; set; }
}