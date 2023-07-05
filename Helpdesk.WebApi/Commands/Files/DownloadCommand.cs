using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Files;

public sealed class DownloadCommand : DataCommand
{
    private const string ContentType = "application/octet-stream";

    private readonly IWebHostEnvironment _webHostEnvironment;

    public DownloadCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<CommandResponseModel<FileDownloadedModel?>> DownloadAsync(string uid)
    {
        var filesRootPath = $"{_webHostEnvironment.WebRootPath}/files";
        var foundFiles = Directory.GetFiles(filesRootPath, $"*.*.{uid}");

        if (!foundFiles.Any())
        {
            await AppDatabaseContext.Set<FileDataModel>()
                .Where(f => f.Uid == uid)
                .ExecuteDeleteAsync();

            return CommandResponse<FileDownloadedModel?>
            (
                errorDetail: $"{Description(typeof(FileDataModel))} не найден."
            );
        }

        var fileIsExisted = await AppDatabaseContext
            .Set<FileDataModel>()
            .AnyAsync(f => f.Uid == uid);
        var fileFullName = foundFiles.First();

        if (!fileIsExisted)
        {
            File.Delete(fileFullName);

            return CommandResponse<FileDownloadedModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(FileDataModel))}' не найдена."
            );
        }

        var fileName = Path.GetFileName(fileFullName.Replace($".{uid}", string.Empty));

        await using var fileStream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read);
        var bytes = new byte[fileStream.Length];
        var actuallyReadBytes = await fileStream.ReadAsync(bytes.AsMemory(default, (int)fileStream.Length));

        if (actuallyReadBytes != (int)fileStream.Length)
        {
            return CommandResponse<FileDownloadedModel?>
            (
                errorDetail: $"{Description(typeof(FileDataModel))} не был выгружен полностью."
            );
        }

        return CommandResponse<FileDownloadedModel?>
        (
            new FileDownloadedModel
            {
                Uid = uid,

                FileName = fileName,

                ContentType = ContentType,

                FileContent = bytes
            }
        );
    }
}