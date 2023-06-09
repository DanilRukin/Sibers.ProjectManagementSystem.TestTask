using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries
{
    public class GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public bool IncludeEmployees { get; }

        public GetAllProjectsQuery(bool includeEmployees)
        {
            IncludeEmployees = includeEmployees;
        }
    }
}
