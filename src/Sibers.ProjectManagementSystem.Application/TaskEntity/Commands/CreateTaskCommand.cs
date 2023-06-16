using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity.Commands
{
    public class CreateTaskCommand : IRequest<Result<TaskDto>>
    {
        [DataMember]
        public int ProjectId { get; private set; }
        [DataMember]
        public int AuthorId { get; private set; }
        [DataMember]
        public TaskDto TaskDto { get; private set; }

        public CreateTaskCommand(int projectId, int authorId, TaskDto taskDto)
        {
            ProjectId = projectId;
            AuthorId = authorId;
            TaskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));
        }
    }
}
