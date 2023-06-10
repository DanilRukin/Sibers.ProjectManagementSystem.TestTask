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

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    internal class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetEmployeeByIdQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? employee;
                if (request.Option.IncludeProjects)
                    employee = await _context.Employees
                        .IncludeProjects()
                        .FirstOrDefaultAsync(e => e.Id == request.Option.EmployeeId, cancellationToken);
                else
                    employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Id == request.Option.EmployeeId, cancellationToken);
                if (employee == null)
                    return Result<EmployeeDto>.NotFound($"No such employee with id: {request.Option.EmployeeId}");
                else
                    return Result.Success(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<EmployeeDto>(ex);
            }
        }
    }
}
