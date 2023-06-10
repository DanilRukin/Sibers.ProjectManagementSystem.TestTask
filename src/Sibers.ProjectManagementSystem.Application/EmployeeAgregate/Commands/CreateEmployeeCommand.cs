using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Commands
{
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto EmployeeDto { get; private set; }

        public CreateEmployeeCommand(EmployeeDto employeeDto)
        {
            EmployeeDto = employeeDto ?? throw new ArgumentNullException(nameof(employeeDto));
        }
    }
}
