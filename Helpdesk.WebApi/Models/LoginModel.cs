using System.ComponentModel.DataAnnotations;

namespace Helpdesk.WebApi.Models;

public class LoginModel
{
    [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public bool IsInternal { get; set; }
}