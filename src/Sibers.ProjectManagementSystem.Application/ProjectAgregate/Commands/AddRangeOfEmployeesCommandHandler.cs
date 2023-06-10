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
    internal class AddRangeOfEmployeesCommandHandler : IRequestHandler<AddRangeOfEmployeesCommand, Result>
    {
        private ProjectManagementSystemContext _context;
        public AddRangeOfEmployeesCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(AddRangeOfEmployeesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context
                    .Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                List<Employee> employees = new List<Employee>();
                Employee? employee;
                foreach (var id in request.EmployeesIds)
                {
                    employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                    if (employee == null)
                        return Result.NotFound($"No such employee with id: {id}");
                    employees.Add(employee);
                }
                foreach (var item in employees)
                {
                    project.AddEmployee(item);
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
