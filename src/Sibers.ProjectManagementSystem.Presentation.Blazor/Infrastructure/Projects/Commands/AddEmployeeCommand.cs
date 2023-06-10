using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class AddEmployeeCommand : IRequest<Result>
    {
        public int ProjectId { get; private set; }
        public int EmployeeId { get; private set; }

        public AddEmployeeCommand(int projectId, int employeeId)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
        }
    }
}
