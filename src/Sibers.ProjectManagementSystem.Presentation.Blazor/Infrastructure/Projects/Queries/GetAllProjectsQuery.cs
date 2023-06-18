using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries
{
    public class GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public bool IncludeEmployees { get; private set; }
        public bool IncludeTasks { get; private set; }

        public GetAllProjectsQuery(bool includeEmployees, bool includeTasks)
        {
            IncludeEmployees = includeEmployees;
            IncludeTasks = includeTasks;
        }
    }
}
