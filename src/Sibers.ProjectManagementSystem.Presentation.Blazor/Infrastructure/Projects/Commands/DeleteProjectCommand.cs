using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DeleteProjectCommand : IRequest<Result>
    {
        public int ProjectId { get; private set; }

        public DeleteProjectCommand(int projectId)
        {
            ProjectId = projectId;
        }
    }
}
