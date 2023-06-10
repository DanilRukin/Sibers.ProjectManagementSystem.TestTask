using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetRangeOfEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public IEnumerable<EmployeeIncludeOptions> Options { get; }

        public GetRangeOfEmployeesQuery(IEnumerable<EmployeeIncludeOptions> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
