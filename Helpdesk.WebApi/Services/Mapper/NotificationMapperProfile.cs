using AutoMapper;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class NotificationMapperProfile : Profile
{
    public NotificationMapperProfile()
    {
        CreateMap<NotificationDataModel, NotificationModel>()
            .ForMember(
                destination => destination.RequirementId,
                config => config.MapFrom(source => source.RequirementLinkNotification != null ? (int?)source.RequirementLinkNotification.RequirementId : null)
            );
    }
}