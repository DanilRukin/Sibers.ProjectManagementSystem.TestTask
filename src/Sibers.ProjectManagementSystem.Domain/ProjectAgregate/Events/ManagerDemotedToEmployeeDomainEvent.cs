using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class ManagerDemotedToEmployeeDomainEvent : DomainEvent
    {
        public int EmployeeId { get; }
        public int ProjectId { get; }
        public string Reason { get; }

        public ManagerDemotedToEmployeeDomainEvent(int employeeId, int projectId, string reason)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
            Reason = reason;
        }
    }
}
