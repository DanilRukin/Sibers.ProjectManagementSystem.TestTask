using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.UnitTests.Domain.FakeFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public abstract class DomainFixture
    {
        protected IEmployeeFactory _employeeFactory = new FakeEmployeeFactory();
        protected IProjectFactory _projectFactory = new FakeProjectFactory();
        protected virtual Project GetNextProject()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);
            return _projectFactory.Create("Test project",
                startDate,
                endDate,
                new Priority(1),
                "CustomerCompany",
                "ContractorCompany");
        }

        protected virtual Employee GetNextEmployee()
        {
            return _employeeFactory.Create(new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
        }
    }
}
