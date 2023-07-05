using System.ComponentModel.DataAnnotations;

namespace Helpdesk.WebApi.Models;

public class UserRegistrationModel
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string ConfirmedPassword { get; set; } = string.Empty;
}

public class CorporateUserActivatorModel
{
    public string Name { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    public string? Password { get; set; } = string.Empty;
}