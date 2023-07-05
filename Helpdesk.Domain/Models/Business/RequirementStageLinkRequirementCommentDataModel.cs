using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Стадия заявки-комментарий")]
public class RequirementStageLinkRequirementCommentDataModel : IEntity
{
    public int Id { get; set; }

    public int RequirementStageId { get; set; }

    public int RequirementCommentId { get; set; }

    public RequirementStageDataModel? RequirementStage { get; set; }

    public RequirementCommentDataModel? RequirementComment { get; set; }
}