using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto EmployeeDto { get; private set; }

        public CreateEmployeeCommand(EmployeeDto employeeDto)
        {
            EmployeeDto = employeeDto ?? throw new ArgumentNullException(nameof(employeeDto));
        }
    }
}
