using AutoMapper;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate
{
    internal class EmployeeAgregateEmployeeDtoProfile : Profile
    {
        public EmployeeAgregateEmployeeDtoProfile() 
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.PersonalData.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(src => src.PersonalData.LastName))
                .ForMember(dto => dto.Patronymic, opt => opt.MapFrom(src => src.PersonalData.Patronymic))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dto => dto.ProjectsIds, opt => opt.MapFrom(src => src.Projects.Select(p => p.Id)))
                ;
        }
    }
}
