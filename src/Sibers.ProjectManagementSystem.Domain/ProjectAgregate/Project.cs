using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskEntity.TaskStatus;

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

        private List<Task> _tasks = new List<Task>();
        public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();

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

        internal void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            _tasks ??= new List<TaskEntity.Task>();
            if (!_tasks.Contains(task))
            {
                _tasks.Add(task);
                AddDomainEvent(new TaskAddedDomainEvent(task, Id));
            }
        }

        public void RemoveTask(Task? task, int employeeId)
        {
            if (task != null)
            {
                _tasks?.Remove(task);
                AddDomainEvent(new TaskRemovedDomainEvent(Id, task, employeeId));
            }
        }

        public void ChangeTasksContractor(Task task, Employee newContractor)
        {
            if (newContractor == null)
                throw new InvalidOperationException("New contractor is null. Use RemoveContractorOfTask() method instead");
            if (!Employees.Contains(newContractor))
                throw new InvalidOperationException($"No such employee (id: {newContractor.Id}) on this project." +
                    $" Employee must work on project to become a contractor.");
            if (task != null)
            {
                _tasks ??= new List<TaskEntity.Task>();
                if (_tasks.Contains(task))
                {
                    Task? toChange = _tasks.Find(t => t.Id == task.Id);
                    toChange.ChangeContractor(newContractor.Id);
                    Employee oldContractor = Employees.FirstOrDefault(e => e.Id == task.ContractorEmployeeId);
                    oldContractor?.StopBeingAContractorOfTask(toChange);
                    newContractor.BecomeAContractorOfTask(toChange);
                    task.ChangeContractor(newContractor.Id);
                    AddDomainEvent(new ContractorChangedDomainEvent(Id, task, newContractor.Id));
                }
                else
                    throw new InvalidOperationException($"No such task (id: {task.Id}) on a project.");
            }
            else
                throw new InvalidOperationException("Task is null.");
        }

        public void RemoveContractorOfTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            _tasks ??= new List<TaskEntity.Task>();
            if (_tasks.Contains(task))
            {
                Task? toChange = _tasks.Find(t => t.Id == task.Id);
                toChange.RemoveContractor();
                Employee oldContractor = Employees.FirstOrDefault(e => e.Id == task.ContractorEmployeeId);
                oldContractor?.StopBeingAContractorOfTask(toChange);
                task.RemoveContractor();
            }
            else
                throw new InvalidOperationException($"No such task (id: {task.Id}) on a project.");
        }

        internal void StartTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Start();
            if (task.TaskStatus != TaskStatus.InProgress)
                task.Start();
        }

        internal void SuspendTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Suspend();
            if (task.TaskStatus != TaskStatus.ToDo)
                task.Suspend();
        }

        internal void CompleteTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Complete();
            if (task.TaskStatus != TaskStatus.Completed)
                task.Complete();
        }

        private void ThrowIfNotValidTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("Task is null");
            _tasks ??= new List<TaskEntity.Task>();
            if (!_tasks.Contains(task))
                throw new InvalidOperationException($"No such task (id: {task.Id}) on project (id: {Id}).");
        }
    }
}
