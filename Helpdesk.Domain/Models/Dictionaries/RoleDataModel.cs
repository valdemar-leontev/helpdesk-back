using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Admin;

namespace Helpdesk.Domain.Models.Dictionaries;

[Table("Role", Schema = "Dictionaries")]
[Description("Роль")]
public class RoleDataModel : DictionaryBaseEntity
{
    public required string Code { get; set; }

    public ICollection<UserDataModel>? Users { get; set; }
}