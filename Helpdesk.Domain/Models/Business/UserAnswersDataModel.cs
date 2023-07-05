using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Ответ пользователя")]
public class UserAnswerDataModel : IEntity
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public int QuestionId { get; set; }

    public int RequirementId { get; set; }

    public required string Value { get; set; }

    public int? VariantId { get; set; }

    public QuestionDataModel? Question { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public RequirementDataModel? Requirement { get; set; }
}