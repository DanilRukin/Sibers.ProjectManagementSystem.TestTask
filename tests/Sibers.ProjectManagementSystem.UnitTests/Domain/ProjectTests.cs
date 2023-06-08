using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class ProjectTests : DomainFixture
    {
        public ProjectTests()
        {
        }

        [Fact]
        public void ChangeStartDate_StartDateIsLaterThanEndDate_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            DateTime laterDate = project.EndDate.AddMinutes(1);

            string message = $"You cannot set start date ('{laterDate}') that is later" +
                    $" then current end date ('{project.EndDate}')";

            var result = Assert.Throws<InvalidOperationException>(() => project.ChangeStartDate(laterDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeStartDate_StartDateIsEqualToEndDate_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            DateTime laterDate = project.EndDate.AddDays(0);

            string message = $"You cannot set start date ('{laterDate}') that is equal to" +
                    $" current end date ('{project.EndDate}')";

            var result = Assert.Throws<InvalidOperationException>(() => project.ChangeStartDate(laterDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeEndDate_EndDateIsElierThanStartDate_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            DateTime elierDate = project.StartDate.AddDays(-1);

            string message = $"You cannot set end date ('{elierDate}') that is elier" +
                $" then current start date ('{project.StartDate}')";

            var result = Assert.Throws<InvalidOperationException>(() => project.ChangeEndDate(elierDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeEndDate_EndDateIsEqualToStartDate_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            DateTime elierDate = project.StartDate.AddDays(0);

            string message = $"You cannot set end date ('{elierDate}') that is equal to" +
                $" current start date ('{project.StartDate}')";

            var result = Assert.Throws<InvalidOperationException>(() => project.ChangeEndDate(elierDate));
            Assert.Equal(message, result.Message);
        }


        [Fact]
        public void AddEmployee_EmployeeWasAddedEarlier_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            string message = $"Such employee (id: {employee.Id}) is already works on this project";

            var ex = Assert.Throws<InvalidOperationException>(() => project.AddEmployee(employee));
            Assert.Equal(message, ex.Message);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);
        }

        [Fact]
        public void AddEmployee_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetNextProject();
            Employee employee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => project.AddEmployee(employee));
        }

        [Fact]
        public void AddEmployee_EmployeeAdded()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();

            project.AddEmployee(employee);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);

            Assert.Contains(project, employee.Projects);
            Assert.Single(employee.Projects);
        }

        [Fact]
        public void RemoveEmployee_NoSuchEmployee_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            Employee emp = GetNextEmployee();
            string message = $"No such employee (id: {emp.Id}) on project";

            var ex = Assert.Throws<InvalidOperationException>(() => project.RemoveEmployee(emp));
            Assert.Equal(message, ex.Message);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);
        }

        [Fact]
        public void RemoveEmployee_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetNextProject();
            Employee employee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => project.RemoveEmployee(employee));
        }

        [Fact]
        public void RemoveEmployee_EmployeeRemoved()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);

            project.RemoveEmployee(employee);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Empty(employees);

            Assert.Empty(employee.Projects);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetNextProject();
            Employee employee = null;

            Assert.Throws<ArgumentNullException>(() => project.PromoteEmployeeToManager(employee));
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsNotWorkOnProject_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            string message = "Current emplyee is not work on this project. Add him/her to project first";

            var ex = Assert.Throws<InvalidOperationException>(() => project.PromoteEmployeeToManager(employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeePromoted()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);

            project.PromoteEmployeeToManager(employee);

            Assert.NotNull(project.Manager);
            Assert.Equal(employee, project.Manager);
            Assert.Contains(project, employee.Projects);
            Assert.Contains(project, employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void PromoteEmployeeToManager_ManagerIsExists_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            string message = "You must demote or fire current manager first";
            Employee employee1 = GetNextEmployee();
            project.AddEmployee(employee1);

            var ex = Assert.Throws<InvalidOperationException>(() => project.PromoteEmployeeToManager(employee1));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryToPromoteTheSameEmployee_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            string message = "This emplyee is already manager";

            var ex = Assert.Throws<InvalidOperationException>(() => project.PromoteEmployeeToManager(employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_NoManager_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<InvalidOperationException>(() => project.DemoteManagerToEmployee("reason"));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ManagerDemoted()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.DemoteManagerToEmployee("reason");

            Assert.Null(project.Manager);
            Assert.Contains(employee, project.Employees);
            Assert.Contains(project, employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void FireManager_NoManager_ThrowsInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<InvalidOperationException>(() => project.FireManager("reason"));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void FireManager_ManagerFired()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.FireManager("reason");

            Assert.Null(project.Manager);
            Assert.DoesNotContain(employee, project.Employees);
            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
        }
    }
}
