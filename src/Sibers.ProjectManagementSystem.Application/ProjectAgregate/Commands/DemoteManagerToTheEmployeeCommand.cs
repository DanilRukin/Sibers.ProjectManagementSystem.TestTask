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
    public class DemoteManagerToTheEmployeeCommand : IRequest<Result>
    {
        public int ProjectId { get; private set; }
        public string Reason { get; private set; }

        public DemoteManagerToTheEmployeeCommand(int projectId, string reason)
        {
            ProjectId = projectId;
            Reason = reason;
        }
    }
}
