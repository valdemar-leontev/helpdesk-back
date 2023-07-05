using System.ComponentModel;

namespace Helpdesk.WebApi.Models;

[Description("Стадия заявки")]
public class RequirementStageModel
{
    public int Id { get; set; }

    public int RequirementId { get; set; }

    public int UserId { get; set; }

    public string? UserName { get; set; }

    public int StateId { get; set; }

    public required string StateName { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public bool WithComment { get; set; }
}