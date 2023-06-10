using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries
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
