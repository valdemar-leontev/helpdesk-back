using Helpdesk.WebApi.Models.Enums;

namespace Helpdesk.WebApi.Models;

public class OrganizationTreeItemModel
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public required string Description { get; set; }

    public OrganizationTreeItemTypes Type { get; set; }
}