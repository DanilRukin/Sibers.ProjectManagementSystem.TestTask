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
    internal class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetAllEmployeesQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Employee> employees;
                IQueryable<Employee> query = _context.Employees.AsQueryable();
                if (request.IncludeProjects)
                    query = query.IncludeProjects();
                if (request.IncludeCreatedTasks)
                    query = query.IncludeCreatedTasks();
                if (request.IncludeExecutableTasks)
                    query = query.IncludeExecutableTasks();
                employees = await query.ToListAsync(cancellationToken);
                if (employees == null)
                    employees = new List<Employee>();
                return Result.Success(_mapper.Map<List<Employee>, IEnumerable<EmployeeDto>>(employees));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<IEnumerable<EmployeeDto>>(ex);
            }
        }
    }
}
