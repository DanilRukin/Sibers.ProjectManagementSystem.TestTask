using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public bool IncludeProjects { get; }

        public GetAllEmployeesQuery(bool includeProjects)
        {
            IncludeProjects = includeProjects;
        }
    }
}
