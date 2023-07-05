using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Категория заявки")]
public class RequirementCategoryDataModel : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public bool HasAgreement { get; set; }

    public int RequirementCategoryTypeId { get; set; }

    public RequirementCategoryTypeDataModel? RequirementCategoryType { get; set; }

    public ICollection<RequirementDataModel>? Requirements { get; set; }

    public ICollection<RequirementCategoryLinkProfileDataModel>? RequirementCategoryLinkProfile { get; set; }
}