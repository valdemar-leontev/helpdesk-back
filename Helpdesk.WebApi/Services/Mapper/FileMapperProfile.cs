using AutoMapper;
using Helpdesk.Domain.Models.Business;
using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Services.Mapper;

public class FileMapperProfile : Profile
{
    public FileMapperProfile(IWebHostEnvironment webHostEnvironment)
    {
        CreateMap<FileDataModel, FileModel>()
            .ForMember(
                destination => destination.Size,
                config => config.MapFrom(source => new FileInfo($"{webHostEnvironment.WebRootPath}/files/{source.Name}.{source.Uid}").Length / 1024)
            )
            .ForMember(
                destination => destination.UserName,
                config => config.MapFrom(source => source.UploadUser != null && source.UploadUser.Profile != null
                    ? $"{source.UploadUser.Profile.FirstName} {source.UploadUser.Profile.LastName}"
                    : string.Empty)
            )
            .ForMember(
                destination => destination.RequirementName,
                config => config.MapFrom(source => source.RequirementLinkFile != null
                    ? source.RequirementLinkFile.Requirement!.Name
                    : string.Empty)
            );
    }
}