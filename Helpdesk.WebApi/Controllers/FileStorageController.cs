using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Commands.Entities;
using Helpdesk.WebApi.Commands.Files;
using Helpdesk.WebApi.Models.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/files")]
public class FileStorageController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileStorageController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("list/{requirementId:int?}")]
    public async Task<IActionResult> GetListAsync([FromServices] GetFileListCommand command, int? requirementId)
    {
        var fileListResponse = await command.GetAsync(requirementId);

        return fileListResponse.Content is not null
            ? Ok(fileListResponse.Content)
            : Problem(fileListResponse.ErrorDetail);
    }

    [HttpPost("upload/{requirementId:int?}")]
    public async Task<IActionResult> UploadAsync([FromServices] UploadCommand command, int? requirementId)
    {
        var fileUploadedResponse = await command.UploadAsync(requirementId);

        return fileUploadedResponse.Content is not null
            ? Ok(fileUploadedResponse.Content)
            : Problem(fileUploadedResponse.ErrorDetail);
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadAsync([FromServices] DownloadCommand command, string uid)
    {
        var downloadedFileDataResponse = await command.DownloadAsync(uid);

        return downloadedFileDataResponse.Content is not null
            ? File(
                downloadedFileDataResponse.Content.FileContent,
                downloadedFileDataResponse.Content.ContentType,
                downloadedFileDataResponse.Content.FileName
            )
            : Problem(downloadedFileDataResponse.ErrorDetail);
    }

    [HttpDelete("{fileId:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteEntitiesCommand command, int fileId)
    {
        // TODO: join with command
        var deletedFileResponse = await command.DeleteAsync(new EntityGetRequestModel
        {
            EntityTypeName = nameof(FileDataModel),
            Filter = $"Id=={fileId}"
        });

        if (deletedFileResponse.Content is null)
        {
            return Problem(deletedFileResponse.ErrorDetail);
        }

        var filesRootPath = $"{_webHostEnvironment.WebRootPath}/files";
        var file = deletedFileResponse.Content.Cast<FileDataModel>().First();
        var filePath = $"{filesRootPath}/{file.Name}.{file.Uid}";

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        return Ok(deletedFileResponse.Content.First());
    }

    [HttpPost("replace/{uid}")]
    // TODO: why with UID
    public async Task<IActionResult> ReplaceAsync([FromServices] ReplaceCommand command, string uid)
    {
        var replaceResponse = await command.PostAsync(uid);

        return replaceResponse.Content is not null
            ? Ok(replaceResponse.Content)
            : Problem(replaceResponse.ErrorDetail);
    }
}