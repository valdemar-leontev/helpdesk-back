using Helpdesk.Domain.Contracts;

namespace Helpdesk.WebApi.Models;

public class ActiveDirectoryUserModel : IEntity
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string ObjectSid { get; set; }
}