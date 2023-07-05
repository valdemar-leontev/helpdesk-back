using System.Security.Cryptography;
using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Commands.Files;

public sealed class UploadCommand : DataCommand
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UploadCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<CommandResponseModel<IEnumerable<FileDataModel>?>> UploadAsync(int? requirementId)
    {
        var now = DateTimeOffset.UtcNow;
        var files = HttpContextAccessor.HttpContext!.Request.Form.Files;
        var fileRoot = $"{_webHostEnvironment.WebRootPath}/files";

        if (files is null || !files.Any())
        {
            return CommandResponse<IEnumerable<FileDataModel>?>
            (
                errorDetail: $"Коллекция сущностей '{Description(typeof(FileDataModel))}' пустая или отсутствует."
            );
        }

        var fileList = new List<FileDataModel>();

        foreach (var uploadedFile in files)
        {
            var uid = Guid.NewGuid().ToString("N");
            var filePath = $"{fileRoot}/{uploadedFile.FileName}.{uid}";

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await uploadedFile.CopyToAsync(fileStream);

            fileStream.Position = default;
            var fileHash = Convert.ToBase64String(await SHA256.HashDataAsync(fileStream));

            fileList.Add(new FileDataModel
            {
                Name = uploadedFile.FileName,
                CreationDate = now,
                Uid = uid,
                UploadUserId = UserId,
                Hash = fileHash,
                RequirementLinkFile = requirementId is not null
                    ? new RequirementLinkFileDataModel
                    {
                        FileId = default,
                        RequirementId = requirementId.Value
                    }
                    : null
            });
        }

        await AppDatabaseContext
            .Set<FileDataModel>()
            .AddRangeAsync(fileList);

        var affectedEntitiesCount = await AppDatabaseContext.SaveChangesAsync();

        if (affectedEntitiesCount == default)
        {
            return CommandResponse<IEnumerable<FileDataModel>?>
            (
                errorDetail: $"Сущность '{Description(typeof(FileDataModel))}' не была сохранена."
            );
        }

        return CommandResponse<IEnumerable<FileDataModel>?>
        (
            fileList
        );
    }
}