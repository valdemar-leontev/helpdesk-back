using Helpdesk.Domain.Contracts;

namespace Helpdesk.WebApi.Models;

public class RequirementCategoryModel : DictionaryBaseEntity
{
    public int RequirementCategoryTypeId { get; set; }

    public required string RequirementCategoryTypeDescription { get; set; }

    public bool HasAgreement { get; set; }
}