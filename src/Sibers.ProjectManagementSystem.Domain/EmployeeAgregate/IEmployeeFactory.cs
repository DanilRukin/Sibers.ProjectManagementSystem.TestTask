using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public interface IEmployeeFactory
    {
        Employee Create(PersonalData personalData, Email email);
    }
}
