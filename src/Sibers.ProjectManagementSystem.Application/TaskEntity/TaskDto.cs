using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskEntity.TaskStatus;

namespace Sibers.ProjectManagementSystem.Application.TaskEntity
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Priority { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public int ProjectId { get; set; }
        public int? ContractorEmployeeId { get; set; }
        public int AuthorEmployeeId { get; set; }
    }
}
