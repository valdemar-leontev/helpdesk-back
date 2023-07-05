using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Комментарий")]
public class RequirementCommentDataModel : IEntity
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public int RequirementId { get; set; }

    public int SenderProfileId { get; set; }

    public RequirementStageLinkRequirementCommentDataModel? RequirementStageLinkRequirementComment { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public RequirementDataModel? Requirement { get; set; }
}