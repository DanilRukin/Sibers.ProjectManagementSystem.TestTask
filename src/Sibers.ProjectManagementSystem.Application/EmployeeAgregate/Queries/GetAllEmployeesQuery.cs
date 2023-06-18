using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class GetAllEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public bool IncludeProjects { get; private set; }
        public bool IncludeCreatedTasks { get; private set; }
        public bool IncludeExecutableTasks{ get; private set; }

        public GetAllEmployeesQuery(bool includeProjects, bool includeCreatedTasks, bool includeExecutableTasks)
        {
            IncludeProjects = includeProjects;
            IncludeCreatedTasks = includeCreatedTasks;
            IncludeExecutableTasks = includeExecutableTasks;
        }
    }
}
