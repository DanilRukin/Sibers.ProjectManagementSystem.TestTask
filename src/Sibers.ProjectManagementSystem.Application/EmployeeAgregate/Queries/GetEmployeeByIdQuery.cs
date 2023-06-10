using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class GetEmployeeByIdQuery : IRequest<Result<EmployeeDto>>
    {
        public EmployeeIncludeOptions Option { get;  }

        public GetEmployeeByIdQuery(EmployeeIncludeOptions option)
        {
            Option = option ?? throw new ArgumentNullException(nameof(option));
        }
    }
}
