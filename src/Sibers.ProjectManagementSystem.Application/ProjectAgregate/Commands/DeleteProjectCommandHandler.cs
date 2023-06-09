using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    internal class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public DeleteProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? toDelete = await _context.Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (toDelete != null)
                {
                    Employee? manager = toDelete.Manager;
                    if (manager != null)
                        toDelete.FireManager("Project was deleted");
                    Employee[] employees = toDelete.Employees.ToArray();
                    foreach (Employee employee in employees)
                    {
                        toDelete.RemoveEmployee(employee);
                    }
                    _context.Projects.Remove(toDelete);
                    await _context.SaveEntitiesAsync(cancellationToken);
                    return Result.Success();
                }
                else
                {
                    return Result.NotFound($"Project with id: {request.ProjectId} was not found");
                }
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle(ex);
            }
        }
    }
}
