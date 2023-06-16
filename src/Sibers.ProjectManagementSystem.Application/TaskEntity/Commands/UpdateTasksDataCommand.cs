using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Commands
{
    public class UpdateTasksDataCommand : IRequest<Result<TaskDto>>
    {
        public TaskDto Task { get; private set; }

        public UpdateTasksDataCommand(TaskDto task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }
    }
}
