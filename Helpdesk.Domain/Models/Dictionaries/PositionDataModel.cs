using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Должность")]
public class PositionDataModel : DictionaryBaseEntity
{
    public ICollection<ProfileDataModel>? Profiles { get; set; }
}