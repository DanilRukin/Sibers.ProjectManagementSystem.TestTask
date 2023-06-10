using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands
{
    public class AddRangeOfEmployeesCommand : IRequest<Result>
    {
        public IEnumerable<int> EmployeesIds { get; private set; }
        public int ProjectId { get; private set; }

        public AddRangeOfEmployeesCommand(IEnumerable<int> employeesIds, int projectId)
        {
            EmployeesIds = employeesIds ?? throw new ArgumentNullException(nameof(employeesIds));
            ProjectId = projectId;
        }
    }
}
