using Helpdesk.Domain.Contracts;

namespace Helpdesk.WebApi.Models;

public class FileModel : IEntity
{
    public int Id { get; set; }

    public required string Uid { get; set; }

    public required string Name { get; set; }

    public int? UploadUserId { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public string RequirementName { get; set; }

    public required long Size { get; set; }

    public string UserName { get; set; }
}

public class FileDownloadedModel : IEntity
{
    public int Id { get; set; }

    public required string Uid { get; set; }

    public required string FileName { get; set; }

    public required string ContentType { get; set; }

    public required byte[] FileContent { get; set; }
}