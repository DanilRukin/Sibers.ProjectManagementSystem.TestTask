using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    public class TransferEmployeeToAnotherProjectCommand : IRequest<Result>
    {
        public int FromProjectId { get; private set; }
        public int ToProjectId { get; private set; }
        public int EmployeeId { get; private set; }

        public TransferEmployeeToAnotherProjectCommand(int fromProjectId, int toProjectId, int employeeId)
        {
            FromProjectId = fromProjectId;
            ToProjectId = toProjectId;
            EmployeeId = employeeId;
        }
    }
}
