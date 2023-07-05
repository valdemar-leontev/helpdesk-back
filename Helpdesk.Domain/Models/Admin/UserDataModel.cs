using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Admin;

[Description("Пользователь")]
public class UserDataModel : IEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public int RoleId { get; set; }

    public string? ObjectSid { get; set; }

    public RoleDataModel? Role { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public ICollection<NotificationDataModel>? Notifications { get; set; }

    public ICollection<UserSessionDataModel>? UserSessions { get; set; }

    public ICollection<FileDataModel>? Files { get; set; }
}