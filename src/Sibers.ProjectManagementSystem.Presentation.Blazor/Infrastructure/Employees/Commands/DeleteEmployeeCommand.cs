using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest<Result>
    {
        public int EmployeeId { get; private set; }

        public DeleteEmployeeCommand(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
