using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Admin;

namespace Helpdesk.Domain.Models.Business;

[Description("Файл")]
public class FileDataModel : IEntity
{
    public int Id { get; set; }

    public required string Uid { get; set; }

    public required string Name { get; set; }

    public int? UploadUserId { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public string? Hash { get; set; }

    public UserDataModel? UploadUser { get; set; }

    public RequirementLinkFileDataModel? RequirementLinkFile { get; set; }
}