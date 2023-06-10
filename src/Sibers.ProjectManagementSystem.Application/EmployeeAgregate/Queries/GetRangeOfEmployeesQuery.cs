using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class GetRangeOfEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public IEnumerable<EmployeeIncludeOptions> Options { get; }

        public GetRangeOfEmployeesQuery(IEnumerable<EmployeeIncludeOptions> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
