using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Состояние заявки")]
public class RequirementStateDataModel : DictionaryBaseEntity
{
    public ICollection<RequirementDataModel>? Requirements { get; set; }
}