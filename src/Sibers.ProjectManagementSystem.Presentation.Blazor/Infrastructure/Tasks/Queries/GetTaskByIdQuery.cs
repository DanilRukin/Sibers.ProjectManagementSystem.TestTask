using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries
{
    public class GetTaskByIdQuery : IRequest<Result<TaskDto>>
    {
        public Guid TaskId { get; }

        public GetTaskByIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
