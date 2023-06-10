namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries
{
    public class ProjectIncludeOptions
    {
        public int ProjectId { get; }
        public bool IncludeEmployees { get; }

        public ProjectIncludeOptions(int projectId, bool includeEmployees)
        {
            ProjectId = projectId;
            IncludeEmployees = includeEmployees;
        }
    }
}
