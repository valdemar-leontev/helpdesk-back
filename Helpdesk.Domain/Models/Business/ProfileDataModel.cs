using System.ComponentModel;
using Helpdesk.Domain.Contracts;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Dictionaries;

namespace Helpdesk.Domain.Models.Business;

[Description("Профиль пользователя")]
public class ProfileDataModel : IEntity
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? PositionId { get; set; }

    public int UserId { get; set; }

    public PositionDataModel? Position { get; set; }

    public ProfileLinkSubdivisionDataModel? ProfileLinkSubdivision { get; set; }

    public UserDataModel? User { get; set; }

    public ICollection<RequirementCategoryLinkProfileDataModel>? RequirementCategoryLinkProfile { get; set; }

    public ICollection<RequirementCommentDataModel>? RequirementComments { get; set; }

    public ICollection<RequirementDataModel>? Requirements { get; set; }

    public ICollection<RequirementStageDataModel>? RequirementStages { get; set; }

    public ICollection<UserAnswerDataModel>? UserAnswers { get; set; }

    public ICollection<RequirementLinkProfileDataModel>? RequirementLinkProfiles { get; set; }
}