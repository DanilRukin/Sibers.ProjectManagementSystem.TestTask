using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    internal class DeleteTaskFromProjectCommandHandler : IRequestHandler<DeleteTaskFromProjectCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public DeleteTaskFromProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteTaskFromProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .IncludeEmployees()
                    .IncludeTasks()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                {
                    return Result.NotFound($"No such project with id: '{request.ProjectId}'");
                }
                project.RemoveTask(project.Tasks.FirstOrDefault(t => t.Id == request.TaskId), request.EmployeeId);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle(ex);
            }
        }
    }
}
