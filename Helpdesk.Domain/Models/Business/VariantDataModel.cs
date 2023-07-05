using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Вариант ответа")]
public class VariantDataModel : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public int? QuestionId { get; set; }

    public QuestionDataModel? Question { get; set; }
}