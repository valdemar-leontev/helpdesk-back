using Helpdesk.Domain.Contracts;

namespace Helpdesk.WebApi.Models;

public class RequirementCategoryTreeItemModel : IEntity
{
    public int Id { get; set; }

    public required string RequirementCategoryDescription { get; set; }

    public int RequirementCategoryTypeId { get; set; }

    public required string RequirementCategoryTypeDescription { get; set; }

    public bool HasAgreement { get; set; }
}