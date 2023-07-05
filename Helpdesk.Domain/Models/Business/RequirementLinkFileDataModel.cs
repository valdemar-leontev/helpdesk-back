using Helpdesk.Domain.Contracts;

namespace Helpdesk.Domain.Models.Business;

public class RequirementLinkFileDataModel : IEntity
{
    public int Id { get; set; }

    public required int RequirementId { get; set; }

    public required int FileId { get; set; }

    public RequirementDataModel? Requirement { get; set; }

    public FileDataModel? File { get; set; }
}