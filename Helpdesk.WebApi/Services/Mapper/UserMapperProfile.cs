using AutoMapper;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<ActiveDirectoryUserModel, UserDataModel>()
            .ForMember(
                destination => destination.Password,
                config => config.MapFrom(source => source.ObjectSid)
            )
            .ForMember(
                destination => destination.RoleId,
                config => config.MapFrom(source => (int)Roles.User)
            );

        CreateMap<UserDataModel, ActiveDirectoryUserModel>();

        CreateMap<UserDataModel, ProfileListItemModel>()
            .ForMember(
                destination => destination.Id,
                config => config.MapFrom(source => source.Profile != null
                    ? source.Profile.Id
                    : default(int))
            )
            .ForMember(
                destination => destination.UserId,
                config => config.MapFrom(source => source.Id)
            )
            .ForMember(
                destination => destination.UserName,
                config => config.MapFrom(source => source.Name)
            )
            .ForMember(
                destination => destination.FirstName,
                config => config.MapFrom(source => source.Profile != null
                    ? source.Profile.FirstName
                    : null)
            )
            .ForMember(
                destination => destination.LastName,
                config => config.MapFrom(source => source.Profile != null
                    ? source.Profile.LastName
                    : null)
            )
            .ForMember(
                destination => destination.PositionId,
                config => config.MapFrom(source => source.Profile != null
                    ? source.Profile.PositionId
                    : null)
            )
            .ForMember(
                destination => destination.SubdivisionId,
                config => config.MapFrom(source =>
                    source.Profile != null && source.Profile.ProfileLinkSubdivision != null
                        ? (int?)source.Profile.ProfileLinkSubdivision.SubdivisionId
                        : null)
            )
            .ForMember(
                destination => destination.PositionName,
                config => config.MapFrom(source => source.Profile != null && source.Profile.Position != null
                    ? source.Profile.Position.Description
                    : null)
            )
            .ForMember(
                destination => destination.SubdivisionName,
                config => config.MapFrom(source => source.Profile!.ProfileLinkSubdivision != null
                    ? source.Profile.ProfileLinkSubdivision.Subdivision!.Description
                    : null)
            )
            .ForMember(
                destination => destination.HasProfile,
                config => config.MapFrom(source => source.Profile != null
                                                   && source.Profile.PositionId != null
                                                   && source.Profile.ProfileLinkSubdivision != null
                                                   && source.Profile.FirstName != null
                                                   && source.Profile.LastName != null)
            ).ForMember(
                destination => destination.IsHead,
                config => config.MapFrom(source => source.Profile != null
                                                   && source.Profile.ProfileLinkSubdivision != null
                                                   && source.Profile.ProfileLinkSubdivision.IsHead)
            );
    }
}