using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries
{
    public class GetRangeOfProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public IEnumerable<ProjectIncludeOptions> Options { get; }

        public GetRangeOfProjectsQuery(IEnumerable<ProjectIncludeOptions> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
