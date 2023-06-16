using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public class Employee : EntityBase<int>, IAgregateRoot
    {
        public PersonalData PersonalData { get; set; }
        public Email Email { get; set; }

        private List<TaskEntity.Task> _createdTasks = new List<TaskEntity.Task>();
        public IReadOnlyCollection<TaskEntity.Task> CreatedTasks => _createdTasks.AsReadOnly();

        private List<TaskEntity.Task> _executableTasks = new List<TaskEntity.Task>();
        public IReadOnlyCollection<TaskEntity.Task> ExecutableTasks => _executableTasks.AsReadOnly();

        public IReadOnlyCollection<Project> OnTheseProjectsIsEmployee => _employeeOnProjects
            .Where(ep => ep.Role == EmployeeRoleOnProject.Employee)
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        public IReadOnlyCollection<Project> OnTheseProjectsIsManager => _employeeOnProjects
            .Where(ep => ep.Role == EmployeeRoleOnProject.Manager)
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        private List<EmployeeOnProject> _employeeOnProjects = new List<EmployeeOnProject>();
        public IReadOnlyCollection<Project> Projects => _employeeOnProjects
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        internal void AddProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();

            if (!_employeeOnProjects.Contains(employeeOnProject))
            {
                _employeeOnProjects.Add(employeeOnProject);
            }
        }

        internal void RemoveProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects?.Remove(employeeOnProject);
        }

        internal void PromoteToManagerOnProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();
            _employeeOnProjects.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Manager);
        }

        internal void DemoteToManagerOnProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();
            _employeeOnProjects.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Employee);
        }

        protected Employee() { }

        public Employee(PersonalData personalData, Email email)
        {
            PersonalData = personalData ?? throw new ArgumentNullException("Personal data is null");
            Email = email ?? throw new ArgumentNullException("Email is null");
        }

        public void ChangeFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is null or empty");
            PersonalData = new PersonalData(firstName, PersonalData.LastName, PersonalData.Patronymic);
        }

        public void ChangeLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is null or empty");
            PersonalData = new PersonalData(PersonalData.FirstName, lastName, PersonalData.Patronymic);
        }

        public void ChangePatronymic(string patronymic)
        {
            if (patronymic == null)
                throw new ArgumentException("Patronymic is null");
            PersonalData = new PersonalData(PersonalData.FirstName, PersonalData.LastName, patronymic);
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Emailis null or empty");
            if (!email.Contains("@"))
                throw new ArgumentException("Email does not contain '@'");
            Email = new Email(email);
        }

        public TaskEntity.Task CreateTask(Project project, string name, Priority priority = null, string description = "")
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));
            TaskEntity.Task result = new TaskEntity.Task(Guid.NewGuid(), name, description, project.Id, Id, priority);
            _createdTasks ??= new List<TaskEntity.Task>();
            if (!_createdTasks.Contains(result))
            {
                _createdTasks.Add(result);
                project.AddTask(result);
            }
            return result.Clone();
        }

        internal void BecomeAContractorOfTask(TaskEntity.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            _executableTasks ??= new List<TaskEntity.Task>();
            if (!_executableTasks.Contains(task))
            {
                _executableTasks.Add(task);
            }
            else
                throw new InvalidOperationException("This employee is already the contractor of this task");
        }

        internal void StopBeingAContractorOfTask(TaskEntity.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            _executableTasks ??= new List<TaskEntity.Task>();
            if (_executableTasks.Contains(task))
            {
                _executableTasks.Remove(task);
            }
            else
                throw new InvalidOperationException($"Employee (id: {Id}) not execute task (id: {task.Id})");
        }

        public void StartTask(Project project, TaskEntity.Task task)
        {
            ThrowIfNotValid(project, task, "start");
            project.StartTask(task);
        }

        public void SuspendTask(Project project, TaskEntity.Task task)
        {
            ThrowIfNotValid(project, task, "suspend");
            project.SuspendTask(task);
        }

        public void CompleteTask(Project project, TaskEntity.Task task)
        {
            ThrowIfNotValid(project, task, "complete");
            //StopBeingAContractorOfTask(task);  // is it true?
            project.CompleteTask(task);
        }

        private void ThrowIfNotValid(Project project, TaskEntity.Task task, string operation)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (project == null)
                throw new ArgumentNullException(nameof(task));
            _executableTasks ??= new List<TaskEntity.Task>();
            if (!_executableTasks.Contains(task))
                throw new InvalidOperationException($"Employee (id: {Id}) can not {operation} task (id: {task.Id}) " +
                    $"because is not a contractor of this task");
        }
    }
}
