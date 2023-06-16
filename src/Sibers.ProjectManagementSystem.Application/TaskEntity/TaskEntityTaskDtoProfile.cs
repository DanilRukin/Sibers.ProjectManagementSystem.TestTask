using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity
{
    internal class TaskEntityTaskDtoProfile : Profile
    {
        public TaskEntityTaskDtoProfile() 
        {
            CreateMap<Domain.TaskEntity.Task, TaskDto>();
        }
    }
}
