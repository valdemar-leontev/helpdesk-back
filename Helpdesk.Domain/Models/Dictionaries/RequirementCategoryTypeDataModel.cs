using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Тип категории заявки")]
public class RequirementCategoryTypeDataModel : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public ICollection<RequirementCategoryDataModel>? RequirementCategories { get; set; }
}