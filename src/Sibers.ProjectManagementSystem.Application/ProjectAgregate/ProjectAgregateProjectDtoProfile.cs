using AutoMapper;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate
{
    internal class ProjectAgregateProjectDtoProfile : Profile
    {
        public ProjectAgregateProjectDtoProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dto => dto.Priority, opt => opt.MapFrom(src => src.Priority.Value))
                .ForMember(dto => dto.ManagerId, opt => opt.MapFrom(src => src.Manager == null ? 0 : src.Manager.Id))
                .ForMember(dto => dto.EmployeesIds, opt => opt.MapFrom(src => src.Employees.Select(e => e.Id)));
        }
    }
}
