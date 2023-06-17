using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Commands
{
    public class UpdateTaskCommand : IRequest<Result<TaskDto>>
    {
        public TaskDto UpdatedTask { get; }

        public UpdateTaskCommand(TaskDto updatedTask)
        {
            UpdatedTask = updatedTask ?? throw new ArgumentNullException(nameof(updatedTask));
        }
    }
}
