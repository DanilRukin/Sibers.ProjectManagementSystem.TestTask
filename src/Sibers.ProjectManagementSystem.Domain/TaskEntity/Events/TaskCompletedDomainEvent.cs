using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.TaskEntity.Events
{
    public class TaskCompletedDomainEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public int ContractorId { get; }
        public int AuthorId { get; }
        public int ProjectId { get; }

        public TaskCompletedDomainEvent(Guid taskId, int contractorId, int authorId, int projectId)
        {
            TaskId = taskId;
            ContractorId = contractorId;
            AuthorId = authorId;
            ProjectId = projectId;
        }
    }
}
