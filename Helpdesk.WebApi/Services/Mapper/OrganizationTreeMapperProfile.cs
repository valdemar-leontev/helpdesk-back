using AutoMapper;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class OrganizationTreeMapperProfile : Profile
{
    public OrganizationTreeMapperProfile()
    {
        CreateMap<SubdivisionLinkSubdivisionDataModel, OrganizationTreeItemModel>()
            .ForMember(
                destination => destination.Description,
                config => config.MapFrom(source => source.Subdivision != null
                    ? source.Subdivision.Description
                    : null)
            )
            .ForMember(
                destination => destination.Id,
                config => config.MapFrom(source => source.SubdivisionId)
            )
            .ForMember(
                destination => destination.ParentId,
                config => config.MapFrom(source => source.SubdivisionParentId)
            );
    }
}