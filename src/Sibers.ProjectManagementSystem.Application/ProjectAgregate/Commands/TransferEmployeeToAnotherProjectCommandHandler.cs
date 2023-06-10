using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    internal class TransferEmployeeToAnotherProjectCommandHandler : IRequestHandler<TransferEmployeeToAnotherProjectCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public TransferEmployeeToAnotherProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(TransferEmployeeToAnotherProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? currentProject = await _context.Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.FromProjectId, cancellationToken);
                if (currentProject == null)
                    return Result.NotFound($"No such project with id: {request.FromProjectId}");
                Project? futureProject = await _context.Projects
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ToProjectId, cancellationToken);
                if (futureProject == null)
                    return Result.NotFound($"No such project with id: {request.ToProjectId}");
                Employee? employee = await _context.Employees
                    .IncludeProjects()
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
                if (employee == null)
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");

                TransferService transferService = new TransferService();
                transferService.TransferEmployeeToAnotherProject(employee, currentProject, futureProject);

                _context.Projects.UpdateRange(currentProject, futureProject);
                _context.Employees.Update(employee);
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
