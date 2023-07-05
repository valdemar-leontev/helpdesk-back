using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Files;

public class ReplaceCommand : DataCommand
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ReplaceCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<CommandResponseModel<FileDataModel?>> PostAsync(string uid)
    {
        var fileDatabaseRecord = await AppDatabaseContext
            .Set<FileDataModel>()
            .Where(f => f.Uid == uid)
            .FirstOrDefaultAsync();

        if (fileDatabaseRecord is null)
        {
            return CommandResponse<FileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(FileDataModel))}' не найдена."
            );
        }

        var filesRootPath = $"{_webHostEnvironment.WebRootPath}/files";
        var foundFiles = Directory.GetFiles(filesRootPath, $"*.*.{uid}");
        var fileFullName = foundFiles.First();
        File.Delete(fileFullName);

        var files = HttpContextAccessor.HttpContext!.Request.Form.Files;
        var now = DateTimeOffset.UtcNow;

        if (files is null || !files.Any())
        {
            return CommandResponse<FileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(FileDataModel))}' пустая или отсутствует."
            );
        }

        var uploadedFile = files.Single();
        var filePath = $"{filesRootPath}/{uploadedFile.FileName}.{uid}";

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await uploadedFile.CopyToAsync(fileStream);

        fileDatabaseRecord.Name = uploadedFile.FileName;
        fileDatabaseRecord.CreationDate = now;

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<FileDataModel?>
            (
                errorDetail: $"Сущность '{Description(typeof(FileDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<FileDataModel?>
        (
            fileDatabaseRecord
        );
    }
}