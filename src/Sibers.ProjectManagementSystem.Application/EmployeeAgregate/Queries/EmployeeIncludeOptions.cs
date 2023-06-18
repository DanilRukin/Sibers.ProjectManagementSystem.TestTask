using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class EmployeeIncludeOptions
    {
        public int EmployeeId { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)
        public bool IncludeProjects { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)
        public bool IncludeCreatedTasks { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)
        public bool IncludeExecutableTasks { get; set; } // свойство должно быть доступным для записи (чтобы работала привязка в контроллерах)

        public EmployeeIncludeOptions(int employeeId, bool includeProjects, bool includeCreatedTasks, bool includeExecutableTasks)
        {
            EmployeeId = employeeId;
            IncludeProjects = includeProjects;
            IncludeCreatedTasks = includeCreatedTasks;
            IncludeExecutableTasks = includeExecutableTasks;
        }

        public EmployeeIncludeOptions() // должен быть открытый конструктор по умолчанию (чтобы работала привязка в контроллерах)
        {
        }
    }
}
