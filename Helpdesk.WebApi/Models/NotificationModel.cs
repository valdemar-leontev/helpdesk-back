using Helpdesk.Domain.Contracts;

namespace Helpdesk.WebApi.Models;

public class NotificationModel : IEntity
{
    public int Id { get; set; }

    public int RecipientUserId { get; set; }

    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public int? RequirementId { get; set; }
}