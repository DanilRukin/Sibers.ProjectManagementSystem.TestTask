using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data
{
    public static class ProjectManagementSystemContextExtensions
    {
        public static IQueryable<Project> IncludeEmployees(this IQueryable<Project> projects)
        {
            return projects.Include(DataConstants.EMPLOYEES_ON_PROJECT);
        }

        public static IQueryable<Employee> IncludeProjects(this IQueryable<Employee> employees)
        {
            return employees.Include(DataConstants.EMPLOYEE_ON_PROJECTS);
        }

        public static IQueryable<Employee> IncludeCreatedTasks(this IQueryable<Employee> employees)
        {
            return employees.Include(DataConstants.CREATED_TASKS);
        }

        public static IQueryable<Employee> IncludeExecutableTasks(this IQueryable<Employee> employees)
        {
            return employees.Include(DataConstants.EXECUTABLE_TASKS);
        }

        public static IQueryable<Project> IncludeTasks(this IQueryable<Project> projects)
        {
            return projects.Include(DataConstants.PROJECT_HAS_TASKS);
        }
    }
}
