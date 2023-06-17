using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees.Queries;
using System.Text;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Employees
{
    public static class ApiHelper
    {
        public static string Api { get; set; } = "api/employees";
        public static class Get
        {
            public static string ById(int employeeId, bool includeProjects = false)
                => $"{Api}/{employeeId}/{includeProjects}";
            public static string All(bool includeAdditionalData = false)
                => $"{Api}/all/{includeAdditionalData}";
            public static string Range(IEnumerable<EmployeeIncludeOptions> options, string optionParameterName)
                => $"{Api}/range?{ToQuery(options, optionParameterName)}";

            private static string ToQuery(IEnumerable<EmployeeIncludeOptions> options, string optionParameterName)
            {
                int index = 0;
                StringBuilder builder = new StringBuilder(options.Count() * optionParameterName.Length);
                foreach (var option in options)
                {
                    builder.Append($"{optionParameterName}[{index}].{nameof(EmployeeIncludeOptions.EmployeeId)}={option.EmployeeId}&" +
                        $"{optionParameterName}[{index}].{nameof(EmployeeIncludeOptions.IncludeProjects)}={option.IncludeProjects}&");
                    index++;
                }
                string result = builder.ToString();
                return result.Remove(result.Length - 1, 1);
            }
        }

        public static class Post
        {
            public static string Create() => $"{Api}";
            public static string CreateTask(int projectId, int employeeId)
                => $"{Api}/CreateTask/{projectId}/{employeeId}";
        }

        public static class Put
        {
            public static string StartTask(Guid taskId, int employeeId, int projectId)
                => $"{Api}/StartTask/{taskId}/{projectId}/{employeeId}";
            public static string CompleteTask(Guid taskId, int employeeId, int projectId)
                => $"{Api}/CompleteTask/{taskId}/{projectId}/{employeeId}";
            public static string Update() => $"{Api}/update";

            public static string UpdateTask() => $"{Api}/updatetask";
        }

        public static class Delete
        {
            public static string ById(int id) => $"{Api}/{id}";
        }
    }
}
