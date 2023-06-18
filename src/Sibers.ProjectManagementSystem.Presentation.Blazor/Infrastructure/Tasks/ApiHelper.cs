using System.Text;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Tasks
{
    public static class ApiHelper
    {
        public static string Api { get; set; } = "api/tasks";

        public static class Get
        {
            public static string ById(Guid id) => $"{Api}/{id}";

            public static string Range(IEnumerable<Guid> ids, string idsName) =>
                $"{Api}/range?{ToQuery(ids, idsName)}";
            private static string ToQuery(IEnumerable<Guid> ids, string idsName)
            {
                StringBuilder builder = new StringBuilder(ids.Count() * idsName.Length);
                foreach (var id in ids)
                {
                    builder.Append($"{idsName}={id}&");
                }
                string result = builder.ToString();
                return result.Remove(result.Length - 1, 1);
            }
        }

        public static class Delete
        {
            public static string ById(Guid taskId) => $"{Api}/{taskId}";
        }
    }
}
