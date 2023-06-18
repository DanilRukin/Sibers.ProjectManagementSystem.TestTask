namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class EmployeeIncludeOptions
    {
        public EmployeeIncludeOptions(int employeeId, bool includeProjects, bool includeCreatedTasks, bool includeExecutableTasks)
        {
            EmployeeId = employeeId;
            IncludeProjects = includeProjects;
            IncludeCreatedTasks = includeCreatedTasks;
            IncludeExecutableTasks = includeExecutableTasks;
        }

        public int EmployeeId { get; }
        public bool IncludeProjects { get; }
        public bool IncludeCreatedTasks { get; }
        public bool IncludeExecutableTasks { get; }
    }
}
