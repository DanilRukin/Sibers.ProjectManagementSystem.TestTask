using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
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
