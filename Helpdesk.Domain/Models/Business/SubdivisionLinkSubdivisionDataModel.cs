using System.ComponentModel;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Подразделение-Подразделение")]
public class SubdivisionLinkSubdivisionDataModel
{
    public int Id { get; set; }

    public int SubdivisionId { get; set; }

    public int? SubdivisionParentId { get; set; }

    public SubdivisionDataModel? Subdivision { get; set; }

    public SubdivisionDataModel? SubdivisionParent { get; set; }
}