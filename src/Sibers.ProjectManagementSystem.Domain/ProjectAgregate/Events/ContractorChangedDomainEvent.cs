using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class ContractorChangedDomainEvent : DomainEvent
    {
        public int ProjectId { get; }
        public TaskEntity.Task Task { get; }
        public int NewContractor { get; }

        public ContractorChangedDomainEvent(int projectId, TaskEntity.Task task, int newContractor)
        {
            ProjectId = projectId;
            Task = task;
            NewContractor = newContractor;
        }
    }
}
