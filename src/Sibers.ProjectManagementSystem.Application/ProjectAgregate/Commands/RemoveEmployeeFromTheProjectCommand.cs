﻿using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    public class RemoveEmployeeFromTheProjectCommand : IRequest<Result>
    {
        public int ProjectId { get; private set; }

        public int EmployeeId { get; private set; }

        public RemoveEmployeeFromTheProjectCommand(int projectId, int employeeId)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
        }
    }
}
