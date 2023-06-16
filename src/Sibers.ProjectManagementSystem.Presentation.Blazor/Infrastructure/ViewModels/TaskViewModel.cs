using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public TaskStatusViewModel TaskStatusViewModel { get; set; } = TaskStatusViewModel.ToDo;
        public int ProjectId { get; set; }
        public int? ContractorEmployeeId { get; set; }
        public int AuthorEmployeeId { get; set; }
    }

    public enum TaskStatusViewModel
    {
        ToDo, InProgress, Completed
    }
}
