using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain.FakeFactories
{
    internal class FakeEmployeeFactory : IEmployeeFactory
    {
        private int _nextId = 1;

        public Employee Create(PersonalData personalData, Email email)
        {
            Employee employee = new Employee(personalData, email);
            PropertyInfo? property = employee.GetType().GetProperty(nameof(Employee.Id));
            if (property != null)
            {
                property.SetValue(employee, _nextId);
                _nextId++;
            }
            return employee;
        }
    }
}
