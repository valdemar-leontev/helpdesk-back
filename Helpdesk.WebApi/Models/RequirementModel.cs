using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.WebApi.Models;

[Description("Заявка")]
public class RequirementModel : IEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int RequirementTemplateId { get; set; }

    public int? RequirementCategoryId { get; set; }

    public int ProfileId { get; set; }

    public int RequirementStateId { get; set; }

    public string? LastStageProfileName { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public string? UserName { get; set; }

    public bool? HasAgreement { get; set; }

    public string? RequirementCategoryDescription { get; set; }

    public string? RequirementCategoryTypeDescription { get; set; }

    public required string RequirementStateDescription { get; set; }

    public bool WithFiles { get; set; }

    public int FileCount { get; set; }

    public bool IsArchive { get; set; }

    public ICollection<UserAnswerDataModel>? UserAnswers { get; set; }

    public RequirementTemplateDataModel? RequirementTemplate { get; set; }
}