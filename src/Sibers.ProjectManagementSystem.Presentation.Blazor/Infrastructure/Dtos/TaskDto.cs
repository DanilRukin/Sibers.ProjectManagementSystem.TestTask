namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public TaskStatus TaskStatus { get; set; } = TaskStatus.ToDo;
        public int ProjectId { get; set; }
        public int? ContractorEmployeeId { get; set; }
        public int AuthorEmployeeId { get; set; }
    }
    public enum TaskStatus
    {
        ToDo, InProgress, Completed
    }
}
