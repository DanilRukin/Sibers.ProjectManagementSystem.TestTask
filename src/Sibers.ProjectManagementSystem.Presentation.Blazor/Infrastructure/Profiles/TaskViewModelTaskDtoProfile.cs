using AutoMapper;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Profiles
{
    public class TaskViewModelTaskDtoProfile : Profile
    {
        public TaskViewModelTaskDtoProfile() 
        {
            CreateMap<Dtos.TaskDto, TaskViewModel>()
                .ForMember(vm => vm.TaskStatusViewModel, opt => opt.MapFrom(dto => dto.TaskStatus))
                .ReverseMap();
        }
    }
}
