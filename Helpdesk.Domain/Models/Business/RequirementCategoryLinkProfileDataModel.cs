using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Категория заявки-Профиль")]
public class RequirementCategoryLinkProfileDataModel : IEntity
{
    public int Id { get; set; }

    public int RequirementCategoryId { get; set; }

    public int ProfileId { get; set; }

    public RequirementCategoryDataModel? RequirementCategory { get; set; }

    public ProfileDataModel? Profile { get; set; }
}