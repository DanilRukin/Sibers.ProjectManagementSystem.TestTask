using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Commands
{
    /// <summary>
    /// Только для административных целей
    /// </summary>
    public class DeleteTaskCommand : IRequest<Result>
    {
        public Guid TaskId { get; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
