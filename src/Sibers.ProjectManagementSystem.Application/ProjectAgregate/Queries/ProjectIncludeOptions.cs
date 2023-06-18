using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Queries
{
    public class ProjectIncludeOptions
    {
        public int ProjectId { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)
        public bool IncludeEmployees { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)
        public bool IncludeTasks { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)

        public ProjectIncludeOptions(int projectId, bool includeEmployees, bool includeTasks)
        {
            ProjectId = projectId;
            IncludeEmployees = includeEmployees;
            IncludeTasks = includeTasks;
        }

        public ProjectIncludeOptions() // должен быть открытый конструктор по умолчанию (чтобы работала привязка в контроллерах)
        {
        }
    }
}
