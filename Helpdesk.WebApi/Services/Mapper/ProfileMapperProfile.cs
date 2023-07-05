using AutoMapper;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class ProfileMapperProfile : Profile
{
    public ProfileMapperProfile()
    {
        CreateMap<ProfileDataModel, ProfileModel>()
            .ForMember(
                destination => destination.Email,
                config => config.MapFrom(source => source.User!.Email
            ));
    }
}