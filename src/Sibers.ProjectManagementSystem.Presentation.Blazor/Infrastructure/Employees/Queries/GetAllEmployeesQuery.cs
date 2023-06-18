using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public bool IncludeProjects { get; }
        public bool IncludeCreatedTasks{ get; }
        public bool IncludeExecutableTasks { get; }

        public GetAllEmployeesQuery(bool includeProjects, bool includeCreatedTasks, bool includeExecutableTasks)
        {
            IncludeCreatedTasks = includeCreatedTasks;
            IncludeExecutableTasks = includeExecutableTasks;
            IncludeProjects = includeProjects;
        }
    }
}
