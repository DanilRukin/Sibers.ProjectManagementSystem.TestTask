using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    public class DeleteTaskFromProjectCommand : IRequest<Result>
    {
        public int ProjectId { get; }
        public int EmployeeId { get; }
        public Guid TaskId { get; }

        public DeleteTaskFromProjectCommand(int projectId, int employeeId, Guid taskId)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
            TaskId = taskId;
        }
    }
}
