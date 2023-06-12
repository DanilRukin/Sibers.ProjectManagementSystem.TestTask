using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Queries
{
    public class EmployeeIncludeOptions
    {
        public int EmployeeId { get; set; } // свойство должно быть доступным для чтения (чтобы работала привязка в контроллерах)
        public bool IncludeProjects { get; set; } // свойство должно быть доступным для чтения (чтобы работала привязка в контроллерах)

        public EmployeeIncludeOptions(int employeeId, bool includeProjects)
        {
            EmployeeId = employeeId;
            IncludeProjects = includeProjects;
        }

        public EmployeeIncludeOptions() // должен быть открытый конструктор по умолчанию (чтобы работала привязка в контроллерах)
        {
        }
    }
}
