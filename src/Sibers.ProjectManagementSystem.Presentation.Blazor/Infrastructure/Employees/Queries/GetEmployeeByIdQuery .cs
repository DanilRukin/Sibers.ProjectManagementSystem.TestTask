using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class GetEmployeeByIdQuery : IRequest<Result<EmployeeDto>>
    {
        public EmployeeIncludeOptions Options { get; }

        public GetEmployeeByIdQuery(EmployeeIncludeOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
