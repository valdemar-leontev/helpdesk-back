using AutoMapper;
using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class LastStageProfileNameResolver : IValueResolver<RequirementDataModel, RequirementModel, string?>
{
    public string? Resolve(RequirementDataModel source, RequirementModel destination, string? destMember, ResolutionContext context)
    {
        if (source.Stages is null)
        {
            return null;
        }

        var profile = source.Stages.OrderByDescending(s => s.CreationDate).FirstOrDefault(s => s.StateId == source.RequirementStateId)?.Profile;

        return profile != null ? $"{profile.FirstName} {profile.LastName}" : null;
    }
}

public class RequirementMapperProfile : Profile
{
    public RequirementMapperProfile()
    {
        CreateMap<RequirementDataModel, RequirementModel>()
            .ForMember(
                destination => destination.Name,
                config => config.MapFrom(source => source.RequirementCategory != null
                    ? $"{source.RequirementCategory.Description} №{source.OutgoingNumber}"
                    : $"Заявка №{source.OutgoingNumber}") // TODO: how to be assigned OutgoingNumber if RequirementCategory is null
            )
            .ForMember(
                destination => destination.RequirementCategoryDescription,
                config => config.MapFrom(source =>
                    source.RequirementCategory != null ? source.RequirementCategory.Description : null)
            )
            .ForMember(
                destination => destination.UserName,
                config => config.MapFrom(source => source.Profile != null ? $"{source.Profile.FirstName} {source.Profile.LastName}" : null)
            )
            .ForMember(
                destination => destination.HasAgreement,
                config => config.MapFrom(source => source.RequirementCategory!.HasAgreement)
            )
            .ForMember(
                destination => destination.WithFiles,
                config => config.MapFrom(source => source.RequirementLinkFiles != null && source.RequirementLinkFiles.Count != default)
            )
            .ForMember(
                destination => destination.IsArchive,
                config => config.MapFrom(source => source.RequirementLinkProfiles != null &&
                                                   source.RequirementLinkProfiles.Select(l => l.IsArchive)
                                                       .FirstOrDefault()
                )
            )
            .ForMember(
                destination => destination.LastStageProfileName,
                config => config.MapFrom(new LastStageProfileNameResolver())
            )
            .ForMember(
                destination => destination.RequirementStateDescription,
                config => config.MapFrom(source => source.RequirementState != null ? source.RequirementState.Description : string.Empty)
            );

        CreateMap<RequirementStageDataModel, RequirementStageModel>()
            .ForMember(
                destination => destination.UserName,
                config => config.MapFrom(source => source.Profile != null ? $"{source.Profile.FirstName} {source.Profile.LastName}" : null)
            )
            .ForMember(
                destination => destination.StateName,
                config => config.MapFrom(source => source.State != null ? source.State.Description : null)
            )
            .ForMember(
                destination => destination.WithComment,
                config => config.MapFrom(source => source.RequirementStageLinkRequirementComment != null)
            );

        CreateMap<RequirementCategoryDataModel, RequirementCategoryModel>()
            .ForMember(
                destination => destination.RequirementCategoryTypeDescription,
                config => config.MapFrom(source => source.RequirementCategoryType!.Description)
            );

        CreateMap<RequirementCategoryDataModel, RequirementCategoryTreeItemModel>()
            .ForMember(
                destination => destination.RequirementCategoryDescription,
                config => config.MapFrom(source => source.Description)
            )
            .ForMember(
                destination => destination.RequirementCategoryTypeDescription,
                config => config.MapFrom(source => source.RequirementCategoryType!.Description)
            )
            .ForMember(
                destination => destination.RequirementCategoryTypeId,
                config => config.MapFrom(source => source.RequirementCategoryType!.Id)
            );

        CreateMap<RequirementCategoryLinkProfileDataModel, ProfileListItemModel>()
            .ForMember(
                destination => destination.FirstName,
                config => config.MapFrom(source => source.Profile!.FirstName)
            )
            .ForMember(
                destination => destination.LastName,
                config => config.MapFrom(source => source.Profile!.LastName)
            )
            .ForMember(
                destination => destination.PositionName,
                config => config.MapFrom(source => source.Profile!.Position != null
                    ? source.Profile!.Position.Description
                    : null)
            )
            .ForMember(
                destination => destination.Id,
                config => config.MapFrom(source => source.ProfileId)
            );
    }
}