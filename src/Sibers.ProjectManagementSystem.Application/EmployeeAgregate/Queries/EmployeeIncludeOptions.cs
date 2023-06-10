using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class EmployeeIncludeOptions
    {
        public int EmployeeId { get; }
        public bool IncludeProjects { get; }

        public EmployeeIncludeOptions(int employeeId, bool includeProjects)
        {
            EmployeeId = employeeId;
            IncludeProjects = includeProjects;
        }
    }
}
