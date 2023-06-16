using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class TaskRemovedDomainEvent : DomainEvent
    {
        public int ProjectId { get; }
        public Task Task { get; }
        public int EmployeeId { get; }

        public TaskRemovedDomainEvent(int projectId, Task task, int employeeId)
        {
            ProjectId = projectId;
            Task = task;
            EmployeeId = employeeId;
        }
    }
}
