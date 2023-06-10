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
    internal class AddEmployeeToTheProjectCommandHandler : IRequestHandler<AddEmployeeToTheProjectCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public AddEmployeeToTheProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(AddEmployeeToTheProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                Employee? employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
                if (employee == null)
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");
                project.AddEmployee(employee);
                _context.Projects.Update(project);
                _context.Employees.Update(employee);
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
