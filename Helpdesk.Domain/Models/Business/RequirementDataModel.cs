using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Заявка")]
public class RequirementDataModel : IEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int RequirementTemplateId { get; set; }

    public int? RequirementCategoryId { get; set; }

    public int ProfileId { get; set; }

    public int RequirementStateId { get; set; }

    public int OutgoingNumber { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public RequirementStateDataModel? RequirementState { get; set; }

    public RequirementTemplateDataModel? RequirementTemplate { get; set; }

    public RequirementCategoryDataModel? RequirementCategory { get; set; }

    public ICollection<UserAnswerDataModel>? UserAnswers { get; set; }

    public ICollection<RequirementStageDataModel>? Stages { get; set; }

    public ICollection<RequirementCommentDataModel>? RequirementComments { get; set; }

    public ICollection<RequirementLinkFileDataModel>? RequirementLinkFiles { get; set; }

    public ICollection<RequirementLinkProfileDataModel>? RequirementLinkProfiles { get; set; }

    public ICollection<RequirementLinkNotificationDataModel>? RequirementLinkNotification { get; set; }
}