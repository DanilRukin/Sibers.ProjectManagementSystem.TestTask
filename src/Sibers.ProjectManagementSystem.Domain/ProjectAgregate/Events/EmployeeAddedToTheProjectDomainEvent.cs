using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class EmployeeAddedToTheProjectDomainEvent : DomainEvent
    {
        public int EmployeeId { get; }
        public int ProjectId { get; }

        public EmployeeAddedToTheProjectDomainEvent(int employeeId, int projectId)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
        }
    }
}
