using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class UpdateEmployeesDataCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto Employee { get; private set; }

        public UpdateEmployeesDataCommand(EmployeeDto employee)
        {
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }
    }
}
