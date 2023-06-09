using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
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
