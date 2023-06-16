using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskEntity.TaskStatus;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class TaskTests : DomainFixture
    {
        [Fact]
        public void Start_NoContractor_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            
            Task task = employee.CreateTask(project, "name");
            string message = "Task can not be started because it has not contractor employee";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Start());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Start_IsAlreadyStarted_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            string message = "Task can not be started because it is already in progress";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Start());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Suspend_TaskWasNotStarted_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            string message = "Can not to suspend task because it was not started or was suspended";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Suspend());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Suspend_TaskIsCompleted_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            task.Complete();
            string message = "Can not to suspend task because it is completed";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Suspend());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Complete_TaskWasNotStarted_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            string message = "Can not complete the task because it was not started";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Complete());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Complete_TaskWasCompletedLater_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            task.Complete();
            string message = "Can not complete the task because it is already completed";

            var ex = Assert.Throws<InvalidOperationException>(() => task.Complete());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_ContractorIsNull_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            string message = "New contractor is null. Use RemoveContractor() method instead.";

            var ex = Assert.Throws<InvalidOperationException>(() => task.ChangeContractor(null));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_TheSameContractor_ThrowInvalidOperationExceptionWithSpecifiedMessage()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id, contractorId = 2;
            
            Task task = employee.CreateTask(project, "name");
            task.ChangeContractor(contractorId);
            string message = "This employee is already the contractor of this task";

            var ex = Assert.Throws<InvalidOperationException>(() => task.ChangeContractor(contractorId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_ContractorChanged()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id, contractorId = 2;
            Task task = employee.CreateTask(project, "name");

            task.ChangeContractor(contractorId);

            Assert.Equal(contractorId, task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_ContractorIsNotExistsYet_DoNothing()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int? contractorId = task.ContractorEmployeeId;

            task.RemoveContractor();

            Assert.Equal(contractorId, task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_ContractorExists_ContractorBecomeNull()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);

            task.RemoveContractor();

            Assert.Null(task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_TasksStatusShouldBeToDo()
        {
            Project project = GetNextProject();
            Employee employee = GetNextEmployee();
            int authorId = employee.Id;
            Task task = employee.CreateTask(project, "name");
            int contractorId = 2;
            task.ChangeContractor(contractorId);

            task.RemoveContractor();

            Assert.Equal(TaskStatus.ToDo, task.TaskStatus);
        }        
    }
}
