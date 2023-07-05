using System.ComponentModel.DataAnnotations;

namespace Helpdesk.WebApi.Models;

public class AuthUserModel
{
    public int RoleId { get; set; }

    public int UserId { get; set; }

    public int? ProfileId { get; set; }

    [Required] public required string Email { get; set; }

    [Required] public required string Token { get; set; }

    [Required] public required string RefreshToken { get; set; }
}