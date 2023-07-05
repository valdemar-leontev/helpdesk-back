using System.ComponentModel;

namespace Helpdesk.Domain.Models.Dictionaries.Enums;

public enum RequirementStates
{
    [Description("Создана")] Created = 1,

    [Description("В рассмотрении")] UnderConsideration,

    [Description("Согласована")] Agreed,

    [Description("В исполнении")] InExecution,

    [Description("Отклонена")] Rejected,

    [Description("Закрыта")] Closed,

    [Description("Выполнена")] Completed,

    [Description("Переназначено")] Reassigned
}