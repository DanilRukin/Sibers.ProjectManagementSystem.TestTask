using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects
{
    public static class ApiHelper
    {
        public static string _api = "api/projects";

        public static class Get
        {
            public static string ById(int id, bool includeEmployees = false, bool includeTasks = false)
                => $"{_api}/{id}/{includeEmployees}/{includeTasks}";
            public static string All(bool includeEmployees = false, bool includeTasks = false)
                => $"{_api}/all?includeEmployees={includeEmployees}&includeTasks={includeTasks}";
            public static string Range(IEnumerable<ProjectIncludeOptions> options, string optionParameterName)
                => $"{_api}/range?{ToQuery(options, optionParameterName)}";
            public static string Tasks(int projectId)
                => $"{_api}/{projectId}/tasks";

            private static string ToQuery(IEnumerable<ProjectIncludeOptions> options, string optionParameterName)
            {
                int index = 0;
                StringBuilder builder = new StringBuilder(options.Count() * optionParameterName.Length);
                foreach (var option in options)
                {
                    builder.Append($"{optionParameterName}[{index}].{nameof(ProjectIncludeOptions.ProjectId)}={option.ProjectId}&" +
                        $"{optionParameterName}[{index}].{nameof(ProjectIncludeOptions.IncludeEmployees)}={option.IncludeEmployees}&" +
                        $"{optionParameterName}[{index}].{nameof(ProjectIncludeOptions.IncludeTasks)}={option.IncludeTasks}&");
                    index++;
                }
                string result = builder.ToString();
                return result.Remove(result.Length - 1, 1);
            }
        }

        public static class Put
        {
            public static string AddEmployee(int projectId, int employeeId)
                => $"{_api}/addemployee/{projectId}/{employeeId}";
            public static string RemoveEmployee(int projectId, int emplyeeId)
                => $"{_api}/removeemployee/{projectId}/{emplyeeId}";
            public static string FireManager(int projectId, string reason)
                => $"{_api}/firemanager/{projectId}/{reason}";
            public static string DemoteManager(int projectId, string reason = null)
                => string.IsNullOrWhiteSpace(reason) ? $"{_api}/demotemanager/{projectId}" : $"{_api}/demotemanager/{projectId}?reason={reason}";
            public static string PromoteEmployeeToManager(int projectId, int employeeId)
                => $"{_api}/promoteemployee/{projectId}/{employeeId}";
            public static string TransferEmployee(int currentProjectId, int futureProjectId, int employeeId)
                => $"{_api}/transferemployee/{currentProjectId}/{futureProjectId}/{employeeId}";
            public static string AddRangeOfEmployees(int projectId)
                => $"{_api}/addrangeofemployees/{projectId}";
            public static string Update() => $"{_api}/update";
            public static string RemoveRangeOfEmployees(int projectId) =>
                $"{_api}/removerangeofemployees/{projectId}";
            public static string RemoveRangeOfTasks(int projectId, int employeeId)
                => $"{_api}/removerangeoftasks/{projectId}/{employeeId}";
        }

        public static class Post
        {
            public static string Create() => _api;
            public static string AddRangeOfTasks(int projectId) => $"{_api}/addrangeoftasks/{projectId}";
        }

        public static class Delete
        {
            public static string ById(int id) => $"{_api}/{id}";
        }
    }
}
