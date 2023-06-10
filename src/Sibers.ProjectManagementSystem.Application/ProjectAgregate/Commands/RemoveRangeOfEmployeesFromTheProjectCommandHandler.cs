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
    internal class RemoveRangeOfEmployeesFromTheProjectCommandHandler : IRequestHandler<RemoveRangeOfEmployeesFromTheProjectCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public RemoveRangeOfEmployeesFromTheProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(RemoveRangeOfEmployeesFromTheProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<Employee> toRemove = new List<Employee>();
                Project? project = await _context.Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                Employee? employee;
                foreach (var id in request.EmployeesIds)
                {
                    employee = await _context.Employees
                        .IncludeProjects()
                        .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                    if (employee == null)
                        return Result.NotFound($"No such employee with id: {id}");
                    else
                        toRemove.Add(employee);
                }
                foreach (var item in toRemove)
                {
                    project.RemoveEmployee(item);
                }
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return ExceptionHandler.Handle(e);
            }
        }
    }
}
