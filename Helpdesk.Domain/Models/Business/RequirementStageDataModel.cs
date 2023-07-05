using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Стадия заявки")]
public class RequirementStageDataModel : IEntity
{
    public int Id { get; set; }

    public int StateId { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public int RequirementId { get; set; }

    public int ProfileId { get; set; }

    public RequirementStageLinkRequirementCommentDataModel? RequirementStageLinkRequirementComment { get; set; }

    public RequirementDataModel? Requirement { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public RequirementStateDataModel? State { get; set; }
}