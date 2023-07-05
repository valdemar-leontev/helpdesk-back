using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Подразделение")]
public class SubdivisionDataModel : DictionaryBaseEntity
{
    public SubdivisionLinkSubdivisionDataModel? SubdivisionParentLinkSubdivision { get; set; }

    public ICollection<ProfileLinkSubdivisionDataModel>? ProfileLinksSubdivision { get; set; }

    public ICollection<SubdivisionLinkSubdivisionDataModel>? SubdivisionChildLinksSubdivision { get; set; }
}