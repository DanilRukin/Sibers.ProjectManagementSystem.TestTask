using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;

namespace Sibers.ProjectManagementSystem.Data
{
    public static class SeedData
    {
        private static bool _empty = false;
        private static bool _created = false;
        public static Project Project1 { get; private set; }
        public static Project Project2 { get; private set; }

        public static Employee Employee1_WorksOnProject1 { get; private set; }
        public static Employee Employee2_WorksOnProject1And2 { get; private set; }
        public static Employee Employee3_WorksOnProject2 { get; private set; }

        public static Task TaskByEmployee1_OnProject1_Employee2_Executor { get; private set; }
        public static Task TaskByEmployee2_OnProject2_Employee3_Executor { get; private set; }
        public static Task TaskByEmployee1_OnProject2_Employee2_Executor { get; private set; }


        public static void ApplyMigrationAndFillDatabase(ProjectManagementSystemContext context)
        {
            context.Database.Migrate();
            ClearDatabase(context);
            FillDatabase(context);
            _created = true;
        }

        public static void InitializeDatabase(ProjectManagementSystemContext context)
        {
            context.Database.EnsureCreated();
            ClearDatabase(context);
            FillDatabase(context);
            _created = true;
        }

        private static void FillDatabase(ProjectManagementSystemContext context)
        {
            if (_empty)
            {
                DateTime startDate = DateTime.Today;
                DateTime endDate = startDate.AddDays(1);

                Project1 = ((IProjectFactory)context).Create(nameof(Project1), startDate, endDate, new Priority(1),
                    $"{nameof(Project1)}_contractorCompany", $"{nameof(Project1)}_customerCompany");
                Project2 = ((IProjectFactory)context).Create(nameof(Project2), startDate, endDate, new Priority(1),
                    $"{nameof(Project2)}_contractorCompany", $"{nameof(Project2)}_customerCompany");

                Employee1_WorksOnProject1 = ((IEmployeeFactory)context).Create(new PersonalData("Ivanov", "Ivan", "Ivanovich"),
                    new Email($"{nameof(Employee1_WorksOnProject1)}@gmail.com"));
                Employee2_WorksOnProject1And2 = ((IEmployeeFactory)context).Create(new PersonalData("Petrov", "Ivan", "Petrovich"),
                    new Email($"{nameof(Employee2_WorksOnProject1And2)}@gmail.com"));
                Employee3_WorksOnProject2 = ((IEmployeeFactory)context).Create(new PersonalData("Sidorov", "Petr", "Ivanovich"),
                    new Email($"{nameof(Employee3_WorksOnProject2)}@gmail.com"));

                Project1.AddEmployee(Employee1_WorksOnProject1);
                Project1.AddEmployee(Employee2_WorksOnProject1And2);
                Project2.AddEmployee(Employee2_WorksOnProject1And2);
                Project2.AddEmployee(Employee3_WorksOnProject2);
                context.SaveChanges();

                TaskByEmployee1_OnProject1_Employee2_Executor = Employee1_WorksOnProject1.CreateTask(Project1, "name");
                Project1.ChangeTasksContractor(TaskByEmployee1_OnProject1_Employee2_Executor, Employee2_WorksOnProject1And2);
                TaskByEmployee1_OnProject2_Employee2_Executor = Employee1_WorksOnProject1.CreateTask(Project2, "name");
                Project2.ChangeTasksContractor(TaskByEmployee1_OnProject2_Employee2_Executor, Employee2_WorksOnProject1And2);
                TaskByEmployee2_OnProject2_Employee3_Executor = Employee2_WorksOnProject1And2.CreateTask(Project2, "name");
                Project2.ChangeTasksContractor(TaskByEmployee2_OnProject2_Employee3_Executor, Employee3_WorksOnProject2);

                context.SaveChanges();

                _empty = false;
            }
        }

        private static void ClearDatabase(ProjectManagementSystemContext context)
        {
            if (context.Projects.Any())
                context.Projects.RemoveRange(context.Projects);
            if (context.Tasks.Any())
                context.Tasks.RemoveRange(context.Tasks);
            if (context.Employees.Any())
                context.Employees.RemoveRange(context.Employees);
            if (context.EmployeesOnProjects.Any())
                context.EmployeesOnProjects.RemoveRange(context.EmployeesOnProjects);
            context.SaveChanges();

            Project1 = null;
            Project2 = null;

            Employee1_WorksOnProject1 = null;
            Employee2_WorksOnProject1And2 = null;
            Employee3_WorksOnProject2 = null;

            TaskByEmployee1_OnProject1_Employee2_Executor = null;
            TaskByEmployee2_OnProject2_Employee3_Executor = null;
            TaskByEmployee1_OnProject2_Employee2_Executor = null;

            _empty = true;
        }

        public static void ResetDatabase(ProjectManagementSystemContext context)
        {
            if (_created)
            {
                ClearDatabase(context);
                FillDatabase(context);
            }
        }
    }
}
