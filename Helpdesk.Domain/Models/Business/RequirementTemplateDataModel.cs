using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Шаблон заявки")]
public class RequirementTemplateDataModel : IEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; } = "Описание";

    public bool HasRequirementCategory { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public DateTimeOffset UpdateDate { get; set; }

    public ICollection<QuestionDataModel>? Questions { get; set; }
}