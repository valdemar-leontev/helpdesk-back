using System.ComponentModel;
using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

[Description("Заявка-профиль")]
public class RequirementLinkProfileDataModel : IEntity
{
    public int Id { get; set; }

    public int RequirementId { get; set; }

    public int ProfileId { get; set; }

    public bool IsArchive { get; set; }

    public RequirementDataModel? Requirement { get; set; }

    public ProfileDataModel? Profile { get; set; }
}