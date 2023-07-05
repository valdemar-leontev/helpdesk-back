namespace Helpdesk.WebApi.Models;

public class ProfileModel
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public required string Email { get; set; }

    public int? SubdivisionId { get; set; }

    public int? PositionId { get; set; }

    public int UserId { get; set; }
}

public class ProfileListItemModel : ProfileModel
{
    public string? SubdivisionName { get; set; }

    public string? PositionName { get; set; }

    public bool HasProfile { get; set; }

    public bool IsHead { get; set; }
}