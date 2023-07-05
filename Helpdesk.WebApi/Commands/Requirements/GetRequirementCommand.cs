using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Commands.Requirements;

public sealed class GetRequirementCommand : DataCommand
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetRequirementCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        : base(appDatabaseContext, mapper, httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<CommandResponseModel<RequirementModel?>> GetAsync(int requirementId)
    {
        var requirementDataRecord = await AppDatabaseContext
            .Set<RequirementDataModel>()
            .Include(r => r.Profile)
            .Include(r => r.UserAnswers)!
            .Include(r => r.RequirementCategory)
            .Include(r => r.RequirementState)
            .SingleAsync(r => r.Id == requirementId);

        var requirement = Mapper.Map<RequirementModel>(requirementDataRecord);

        var fileRecords = await AppDatabaseContext
            .Set<FileDataModel>()
            .Include(f => f.RequirementLinkFile)
            .Where(f => f.RequirementLinkFile != null && f.RequirementLinkFile.RequirementId == requirementId)
            .ToListAsync();

        var fileFolderNames = Directory
            .GetFiles($"{_webHostEnvironment.WebRootPath}/files/")
            .Select(Path.GetFileName);

        var validateFileRecordsCount = fileRecords
            .Count(df => fileFolderNames.Any(f => $"{df.Name}.{df.Uid}" == f));

        requirement.FileCount = validateFileRecordsCount;

        return CommandResponse<RequirementModel?>
        (
            requirement
        );
    }
}