using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public class Project : EntityBase<int>, IAgregateRoot
    {
        public string Name { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public Priority Priority { get; protected set; }
        public string NameOfTheCustomerCompany { get; protected set; }
        public string NameOfTheContractorCompany { get; protected set; }

        private List<EmployeeOnProject> _employeesOnProject = new List<EmployeeOnProject>();
        public IReadOnlyCollection<Employee> Employees => _employeesOnProject
            .Select(ep => ep.Employee)
            .ToList()
            .AsReadOnly();

        public Employee? Manager => _employeesOnProject
            ?.Where(ep => ep.Role == EmployeeRoleOnProject.Manager)
            ?.Select(ep => ep.Employee)
            ?.FirstOrDefault();

        protected Project()
        {

        }

        public Project(string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            if (startDate >= endDate)
                throw new ArgumentException($"Start date ('{startDate}') is later or equal to end date ('{endDate}')");
            StartDate = startDate;
            EndDate = endDate;
            ChangeName(name);
            ChangePriority(priority);
            ChangeCustomerCompanyName(nameOfTheCustomerCompany);
            ChangeContractorCompanyName(nameOfTheContractorComapny);
        }

        public void AddEmployee(Employee employee)
        {
            if (employee != null)
            {
                _employeesOnProject ??= new List<EmployeeOnProject>();

                EmployeeOnProject employeeOnProject = new EmployeeOnProject(employee, this, EmployeeRoleOnProject.Employee);
                if (!_employeesOnProject.Contains(employeeOnProject))
                {
                    _employeesOnProject.Add(employeeOnProject);
                    employee.AddProject(employeeOnProject);
                    AddDomainEvent(new EmployeeAddedToTheProjectDomainEvent(employee.Id, Id));
                }
                else
                    throw new InvalidOperationException($"Such employee (id: {employee.Id}) is already works on this project");
            }
            else
                throw new ArgumentNullException(nameof(employee));
        }

        public void RemoveEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(Employee));
            EmployeeOnProject? employeeOnProject = _employeesOnProject?
                .FirstOrDefault(ep => ep.Employee.Id == employee.Id
                && ep.Project.Id == this.Id);
            if (employeeOnProject != null)
            {
                _employeesOnProject?.Remove(employeeOnProject);
                employee.RemoveProject(employeeOnProject);
                AddDomainEvent(new EmployeeRemovedFromTheProjectDomainEvent(employee.Id, Id));
            }
            else
                throw new InvalidOperationException($"No such employee (id: {employee.Id}) on project");
        }

        public void PromoteEmployeeToManager(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            Employee manager = Manager;
            if (employee.Equals(manager))
                throw new InvalidOperationException("This emplyee is already manager");
            if (manager != null)
                throw new InvalidOperationException("You must demote or fire current manager first");
            _employeesOnProject ??= new List<EmployeeOnProject>();
            EmployeeOnProject employeeOnProject = new EmployeeOnProject(employee, this, EmployeeRoleOnProject.Employee);
            if (!_employeesOnProject.Contains(employeeOnProject))
                throw new InvalidOperationException("Current emplyee is not work on this project. Add him/her to project first");
            _employeesOnProject.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Manager);
            employee.PromoteToManagerOnProject(employeeOnProject);
            AddDomainEvent(new EmployeePromotedToManagerDomainEvent(employee.Id, Id));
        }

        public void DemoteManagerToEmployee(string reason)
        {
            var manager = Manager;
            if (manager != null)
            {
                EmployeeOnProject managerOnProject = new EmployeeOnProject(manager, this, EmployeeRoleOnProject.Manager);
                _employeesOnProject.First(ep => ep.Equals(managerOnProject)).ChangeRole(EmployeeRoleOnProject.Employee);
                manager.DemoteToManagerOnProject(managerOnProject);
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(manager.Id, Id, reason));
            }
            else
                throw new InvalidOperationException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public void FireManager(string reason)
        {
            var manager = Manager;
            if (manager != null)
            {
                EmployeeOnProject managerOnProject = new EmployeeOnProject(manager, this, EmployeeRoleOnProject.Manager);
                manager.RemoveProject(managerOnProject);
                _employeesOnProject?.Remove(managerOnProject);
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(manager.Id, Id, reason));
            }
            else
                throw new InvalidOperationException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public void ChangeCustomerCompanyName(string name)
        {
            if (name == null)
                throw new ArgumentException("Customer's company name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer's company name cannot be empty or white space");
            NameOfTheCustomerCompany = name;
        }

        public void ChangeContractorCompanyName(string name)
        {
            if (name == null)
                throw new ArgumentException("Contractor's company name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Contractor's company name cannot be empty or white space");
            NameOfTheContractorCompany = name;
        }

        public void ChangeName(string name)
        {
            if (name == null)
                throw new ArgumentException("Project's name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project's name cannot be empty or white space");
            Name = name;
        }

        public void ChangeStartDate(DateTime startDate)
        {
            if (startDate > EndDate)
                throw new InvalidOperationException($"You cannot set start date ('{startDate}') that is later" +
                    $" then current end date ('{EndDate}')");
            if (startDate == EndDate)
                throw new InvalidOperationException($"You cannot set start date ('{startDate}') that is equal to" +
                    $" current end date ('{EndDate}')");
            StartDate = startDate;
        }

        public void ChangeEndDate(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new InvalidOperationException($"You cannot set end date ('{endDate}') that is elier" +
                $" then current start date ('{StartDate}')");
            if (endDate == StartDate)
                throw new InvalidOperationException($"You cannot set end date ('{endDate}') that is equal to" +
                $" current start date ('{StartDate}')");
            EndDate = endDate;
        }

        public void ChangePriority(Priority priority)
        {
            if (priority == null)
                throw new ArgumentException("Priority cannot be null");
            Priority = priority;
        }
    }
}
