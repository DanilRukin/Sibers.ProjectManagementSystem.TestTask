using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class EmployeePromotedToManagerDomainEvent : DomainEvent
    {
        public int EmployeeId { get; }
        public int ProjectId { get; }

        public EmployeePromotedToManagerDomainEvent(int employeeId, int projectId)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
        }
    }
}
