using AutoMapper;
using DispatcherApp.Common.DTOs.Assignment;
using DispatcherApp.Common.DTOs.Files;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.DTOs.Tutorial;
using DispatcherApp.Common.DTOs.User;
using DispatcherApp.Common.Entities;
using Microsoft.AspNetCore.Identity;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.BLL.Common.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tutorial, TutorialResponse>();
        CreateMap<Tutorial, CreateTutorialResponse>();
        CreateMap<CreateTutorialRequest, Tutorial>();

        CreateMap<Assignment, AssignmentResponse>();
        CreateMap<Assignment, AssignmentWithUsersResponse>();

        CreateMap<IdentityUser, UserResponse>();
        CreateMap<IdentityUser, UserInfoResponse>();

        CreateMap<File, FileResponse>()
            .ForMember(
            x => x.FileName,
            i => i.MapFrom(src => src.OriginalFileName));
        CreateMap<File, FileMetadataResponse>()
            .ForMember(
            f => f.CreatedByName,
            x => x.MapFrom(src => src.UploadedByUser.UserName))
            .ForMember(
            f => f.FileName,
            x => x.MapFrom(src => src.OriginalFileName));

        CreateMap<DispatcherSession, SessionResponse>();
    }
}
