using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;

namespace Helpdesk.WebApi.Models;

public class RequirementCommentStateModel
{
    public RequirementStates State { get; set; }

    public RequirementCommentDataModel? RequirementComment { get; set; }
}