using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Commands
{
    internal class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result>
    {
        private ProjectManagementSystemContext _context;

        public DeleteEmployeeCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? toDelete = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
                if (toDelete != null)
                {
                    _context.Employees.Remove(toDelete);
                    _context.SaveChanges();
                    return Result.Success();
                }
                else
                {
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");
                }
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle(ex);
            }
        }
    }
}
