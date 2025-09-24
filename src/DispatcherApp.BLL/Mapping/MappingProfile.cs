using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DispatcherApp.Models.DTOs.Assignment;
using DispatcherApp.Models.DTOs.Tutorial;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Entities.File, FileResponse>()
            .ForMember(
            x => x.FileName, 
            i => i.MapFrom(src => src.OriginalFileName));

        CreateMap<Tutorial, TutorialResponse>();
        CreateMap<CreateTutorialRequest, Tutorial>();
        CreateMap<Assignment, AssignmentResponse>();
        CreateMap<Assignment, AssignmentWithUsersResponse>();
        CreateMap<IdentityUser, UserResponse>();
    }
}
