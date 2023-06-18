using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DeleteTaskFromProjectCommand : IRequest<Result>
    {
        public int ProjectId { get; }
        public int EmployeeId { get; }
        public Guid TaskId { get; }

        public DeleteTaskFromProjectCommand(int projectId, int employeeId, Guid taskId)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
            TaskId = taskId;
        }
    }
}
