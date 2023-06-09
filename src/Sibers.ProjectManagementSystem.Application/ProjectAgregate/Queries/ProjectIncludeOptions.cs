using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries
{
    public class ProjectIncludeOptions
    {
        public int ProjectId { get; }
        public bool IncludeEmployees { get; }

        public ProjectIncludeOptions(int projectId, bool includeEmployees)
        {
            ProjectId = projectId;
            IncludeEmployees = includeEmployees;
        }
    }
}
