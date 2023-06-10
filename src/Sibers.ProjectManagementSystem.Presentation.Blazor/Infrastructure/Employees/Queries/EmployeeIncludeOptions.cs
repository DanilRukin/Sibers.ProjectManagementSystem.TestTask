namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries
{
    public class EmployeeIncludeOptions
    {
        public EmployeeIncludeOptions(int employeeId, bool includeProjects)
        {
            EmployeeId = employeeId;
            IncludeProjects = includeProjects;
        }

        public int EmployeeId { get; }
        public bool IncludeProjects { get; }
    }
}
