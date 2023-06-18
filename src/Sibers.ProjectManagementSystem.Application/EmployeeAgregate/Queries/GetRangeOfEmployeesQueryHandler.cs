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
    internal class GetRangeOfEmployeesQueryHandler : IRequestHandler<GetRangeOfEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public GetRangeOfEmployeesQueryHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetRangeOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? employee;
                List<Employee> selectedEmployees = new List<Employee>();
                foreach (var option in request.Options)
                {
                    employee = null;
                    IQueryable<Employee> query = _context.Employees.AsQueryable();
                    if (option.IncludeProjects)
                        query = query.IncludeProjects();
                    if (option.IncludeCreatedTasks)
                        query = query.IncludeCreatedTasks();
                    if (option.IncludeExecutableTasks)
                        query = query.IncludeExecutableTasks();
                    employee = await query.FirstOrDefaultAsync(e => e.Id == option.EmployeeId, cancellationToken);
                    if (employee != null)
                        selectedEmployees.Add(employee);
                }
                return Result.Success(_mapper.Map<List<Employee>, IEnumerable<EmployeeDto>>(selectedEmployees));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<IEnumerable<EmployeeDto>>(ex);
            }
        }
    }
}
