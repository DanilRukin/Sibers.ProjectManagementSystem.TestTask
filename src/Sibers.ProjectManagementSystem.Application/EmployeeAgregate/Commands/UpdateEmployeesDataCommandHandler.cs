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
    internal class UpdateEmployeesDataCommandHandler : IRequestHandler<UpdateEmployeesDataCommand, Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public UpdateEmployeesDataCommandHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<EmployeeDto>> Handle(UpdateEmployeesDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.Employee.Id, cancellationToken);
                if (employee == null)
                    return Result<EmployeeDto>.NotFound($"No such employee with id: {request.Employee.Id}");
                employee.ChangeFirstName(request.Employee.FirstName);
                employee.ChangeLastName(request.Employee.LastName);
                employee.ChangeEmail(request.Employee.Email);
                employee.ChangePatronymic(request.Employee.Patronymic);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception e)
            {
                return ExceptionHandler.Handle<EmployeeDto>(e);
            }
        }
    }
}
