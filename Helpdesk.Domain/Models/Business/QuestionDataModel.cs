using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Вопрос")]
public class QuestionDataModel : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public bool IsRequired { get; set; }

    public int QuestionTypeId { get; set; }

    public int RequirementTemplateId { get; set; }

    public QuestionTypeDataModel? QuestionType { get; set; }

    public RequirementTemplateDataModel? RequirementTemplate { get; set; }

    public ICollection<VariantDataModel>? Variants { get; set; }

    public ICollection<UserAnswerDataModel>? UserAnswers { get; set; }
}