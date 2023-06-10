using AutoMapper;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Profiles
{
    public class ProjectViewModelProjectDtoProfile : Profile
    {
        public ProjectViewModelProjectDtoProfile()
        {
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
        }
    }
}
