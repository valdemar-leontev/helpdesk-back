using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Admin;

[Description("Сессия пользователя")]
public class UserSessionDataModel : IEntity
{
    public int Id { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public int UserId { get; set; }

    public DateTimeOffset LoginDate { get; set; } = DateTimeOffset.UtcNow;

    public UserDataModel? User { get; set; }
}