using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Queries
{
    public class GetTaskByIdQuery : IRequest<Result<TaskDto>>
    {
        public Guid Taskid { get; }

        public GetTaskByIdQuery(Guid taskid)
        {
            Taskid = taskid;
        }
    }
}
