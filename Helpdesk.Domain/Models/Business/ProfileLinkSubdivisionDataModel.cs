using System.ComponentModel;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Профиль-Подразделение")]
public class ProfileLinkSubdivisionDataModel
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public int SubdivisionId { get; set; }

    public bool IsHead { get; set; }

    public ProfileDataModel? Profile { get; set; }

    public SubdivisionDataModel? Subdivision { get; set; }
}