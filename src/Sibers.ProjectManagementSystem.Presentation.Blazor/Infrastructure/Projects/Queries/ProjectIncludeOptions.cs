namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries
{
    public class ProjectIncludeOptions
    {
        public int ProjectId { get; }
        public bool IncludeEmployees { get; }
        public bool IncludeTasks { get; }

        public ProjectIncludeOptions(int projectId, bool includeEmployees, bool includeTasks)
        {
            ProjectId = projectId;
            IncludeEmployees = includeEmployees;
            IncludeTasks = includeTasks;
        }
    }
}
