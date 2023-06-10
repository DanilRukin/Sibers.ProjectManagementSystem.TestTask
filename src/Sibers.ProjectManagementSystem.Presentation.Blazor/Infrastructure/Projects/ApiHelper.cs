using System.Text;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects
{
    public static class ApiHelper
    {
        public static string _api = "api/projects";

        public static class Get
        {
            public static string ById(int id, bool includeEmployees = false)
                => $"{_api}/{id}/{includeEmployees}";
            public static string All(bool includeEmployees = false)
                => $"{_api}/all?includeEmployees={includeEmployees}";
            public static string Range()
                => $"{_api}/range";
            public static string Tasks(int projectId)
                => $"{_api}/{projectId}/tasks";

            private static string ToQuery(IEnumerable<int> ids, string idsName)
            {
                StringBuilder builder = new StringBuilder(ids.Count() * idsName.Length);
                foreach (var id in ids)
                {
                    builder.Append($"{idsName}={id}&");
                };
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
