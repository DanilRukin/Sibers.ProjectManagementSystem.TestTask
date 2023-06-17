using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Queries
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
