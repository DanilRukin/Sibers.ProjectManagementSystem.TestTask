using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class DemoteManagerToEmployeeCommand : IRequest<Result>
    {
        public int ProjectId { get; private set; }
        public string Reason { get; private set; }

        public DemoteManagerToEmployeeCommand(int projectId, string reason)
        {
            ProjectId = projectId;
            Reason = reason;
        }
    }
}
