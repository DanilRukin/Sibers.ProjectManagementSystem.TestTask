using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks.Queries
{
    public class GetRangeOfTasksQuery : IRequest<Result<IEnumerable<TaskDto>>>
    {
        public IEnumerable<Guid> TasksIds { get; }

        public GetRangeOfTasksQuery(IEnumerable<Guid> tasksIds)
        {
            TasksIds = tasksIds ?? throw new ArgumentNullException(nameof(tasksIds));
        }
    }
}
