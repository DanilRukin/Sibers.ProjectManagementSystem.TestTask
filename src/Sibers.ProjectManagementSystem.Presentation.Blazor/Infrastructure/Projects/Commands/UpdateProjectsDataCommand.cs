using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class UpdateProjectsDataCommand : IRequest<Result<ProjectDto>>
    {
        public ProjectDto Project { get; private set; }

        public UpdateProjectsDataCommand(ProjectDto project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }
}
