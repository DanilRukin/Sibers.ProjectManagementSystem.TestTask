using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class TaskAddedDomainEvent : DomainEvent
    {
        public Task Task { get; }
        public int ProjectId { get; }

        public TaskAddedDomainEvent(Task task, int projectId)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
            ProjectId = projectId;
        }
    }
}
