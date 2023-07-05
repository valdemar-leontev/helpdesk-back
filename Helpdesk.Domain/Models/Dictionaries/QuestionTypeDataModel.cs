using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Business;

namespace Helpdesk.Domain.Models.Dictionaries;

[Description("Тип вопроса")]
public class QuestionTypeDataModel : DictionaryBaseEntity
{
    public ICollection<QuestionDataModel>? Questions { get; set; }
}