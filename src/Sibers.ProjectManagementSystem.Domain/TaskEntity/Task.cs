using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.TaskEntity.Events;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sibers.ProjectManagementSystem.Domain.TaskEntity
{
    public class Task : EntityBase<Guid>
    {
        public string Name { get; protected set; }
        public string? Description { get; protected set; }
        public Priority Priority { get; protected set; }
        public TaskStatus TaskStatus { get; protected set; }
        public int ProjectId { get; protected set; }
        public int? ContractorEmployeeId { get; protected set; }
        public int AuthorEmployeeId { get; protected set; }

        protected Task() { }

        internal Task(Guid id, string name, string description, int projectId, int authorEmployeeId, Priority priority = null, int? contractorEmployeeId = null)
        {
            Id = id;
            ChangeName(name);
            ChangeDescription(description);
            ProjectId = projectId;
            AuthorEmployeeId = authorEmployeeId;
            if (priority == null)
                priority = Priority.Default();
            ChangePriority(priority);
            TaskStatus = TaskStatus.ToDo;
            ContractorEmployeeId = contractorEmployeeId;
        }

        public void ChangeContractor(int? contractorId)
        {
            if (contractorId == null)
                throw new InvalidOperationException("New contractor is null. Use RemoveContractor() method instead.");
            if (ContractorEmployeeId != null && ContractorEmployeeId.Equals(contractorId))
                throw new InvalidOperationException("This employee is already the contractor of this task");
            ContractorEmployeeId = contractorId;
        }

        public void ChangePriority(Priority priority)
        {
            if (priority == null)
                throw new InvalidOperationException("Priority cannot be null");
            Priority = priority;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Task's name can not be null or empty");
            Name = name;
        }

        public void ChangeDescription(string description)
        {
            if (description == null)
                throw new InvalidOperationException("Task's description can not be null");
            Description = description;
        }

        public void Start()
        {
            if (ContractorEmployeeId == null)
                throw new InvalidOperationException("Task can not be started because it has not contractor employee");
            if (TaskStatus == TaskStatus.InProgress)
                throw new InvalidOperationException("Task can not be started because it is already in progress");
            TaskStatus = TaskStatus.InProgress;
            AddDomainEvent(new TaskStartedDomainEvent(Id, ContractorEmployeeId == null ? 0 : (int)ContractorEmployeeId, AuthorEmployeeId, ProjectId));
        }

        public void Suspend()
        {
            if (TaskStatus == TaskStatus.ToDo)
                throw new InvalidOperationException("Can not to suspend task because it was not started or was suspended");
            if (TaskStatus == TaskStatus.Completed)
                throw new InvalidOperationException("Can not to suspend task because it is completed");
            TaskStatus = TaskStatus.ToDo;
            AddDomainEvent(new TaskSuspendedDomainEvent(Id, ContractorEmployeeId == null ? 0 : (int)ContractorEmployeeId, AuthorEmployeeId, ProjectId));
        }

        public void Complete()
        {
            if (TaskStatus == TaskStatus.ToDo)
                throw new InvalidOperationException("Can not complete the task because it was not started");
            if (TaskStatus == TaskStatus.Completed)
                throw new InvalidOperationException("Can not complete the task because it is already completed");
            TaskStatus = TaskStatus.Completed;
            AddDomainEvent(new TaskCompletedDomainEvent(Id, ContractorEmployeeId == null ? 0 : (int)ContractorEmployeeId, AuthorEmployeeId, ProjectId));
        }

        public Task Clone()
        {
            return new Task(Id,
                (string)Name.Clone(),
                (string)Description.Clone(),
                ProjectId,
                AuthorEmployeeId,
                new Priority(Priority.Value),
                ContractorEmployeeId);
        }

        public void RemoveContractor()
        {
            if (ContractorEmployeeId != null)
            {
                ContractorEmployeeId = null;
                TaskStatus = TaskStatus.ToDo;
            }
        }
    }
}
