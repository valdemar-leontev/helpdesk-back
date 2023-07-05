using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Files;

public class GetFileListCommand : DataCommand
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetFileListCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<CommandResponseModel<IEnumerable<FileModel?>>> GetAsync(int? requirementId)
    {
        var user = await AppDatabaseContext
            .Set<UserDataModel>()
            .Where(u => u.Id == UserId)
            .FirstOrDefaultAsync();

        var fileDataRecords = await AppDatabaseContext
            .Set<FileDataModel>()
            .Include(f => f.UploadUser)
            .ThenInclude(u => u!.Profile)
            .Include(f => f.RequirementLinkFile)
            .ThenInclude(l => l!.Requirement)
            .Where(f => (requirementId == null && (user!.RoleId != (int)Roles.User || f.UploadUserId == UserId)) ||
                        (f.RequirementLinkFile != null && f.RequirementLinkFile.RequirementId == requirementId)
            )
            .OrderByDescending(f => f.CreationDate)
            .ToArrayAsync();

        var fileFolderNames = Directory
            .GetFiles($"{_webHostEnvironment.WebRootPath}/files/")
            .Select(Path.GetFileName);

        var validateFileRecords = fileDataRecords
            .Where(df => fileFolderNames.Any(f => $"{df.Name}.{df.Uid}" == f))
            .Select(f => Mapper.Map<FileModel>(f))
            .ToArray();

        return CommandResponse<IEnumerable<FileModel?>>
        (
            validateFileRecords
        );
    }
}