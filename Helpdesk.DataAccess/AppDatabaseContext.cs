using Helpdesk.DataAccess.ModelBuilders.Admin;
using Helpdesk.DataAccess.ModelBuilders.Business;
using Helpdesk.DataAccess.ModelBuilders.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess;

public class AppDatabaseContext : DbContext
{
    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            // Admin
            .BuildUserModel()
            .BuildUserSessionModel()

            // Dictionaries
            .BuildQuestionTypeModel()
            .BuildRoleModel()
            .BuildRequirementStateModel()
            .BuildProfileModel()
            .BuildRequirementCategoryModel()
            .BuildNotificationModel()
            .BuildRequirementStageModel()
            .BuildRequirementCategoryTypeModel()
            // Business
            .BuildRequirementTemplateModel()
            .BuildQuestionModel()
            .BuildVariantModel()
            .BuildUserAnswerModel()
            .BuildRequirementModel()
            .BuildSubdivisionModel()
            .BuildPositionModel()
            .BuildProfileLinkSubdivisionModel()
            .BuildRequirementCategoryLinkProfileModel()
            .BuildRequirementLinkFileModel()
            .BuildSubdivisionLinkSubdivisionModel()
            .BuildRequirementCommentModel()
            .BuildRequirementStageLinkRequirementCommentModel()
            .BuildFileModel()
            .BuildRequirementLinkProfileModel()
            .BuildRequirementLinkNotificationModel();
    }
}