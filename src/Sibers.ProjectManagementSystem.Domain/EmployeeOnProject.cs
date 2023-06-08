using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain
{
    public class EmployeeOnProject : ValueObject
    {
        public Employee Employee { get; protected set; }
        public int EmployeeId { get; protected set; }  // I don't know how avoid this using EF Core

        public Project Project { get; protected set; }
        public int ProjectId { get; protected set; }  // I don't know how avoid this using EF Core

        public EmployeeRoleOnProject Role { get; protected set; }
        protected EmployeeOnProject() { }

        internal EmployeeOnProject(Employee employee, Project project, EmployeeRoleOnProject role)
        {
            Employee = employee;
            Project = project;
            Role = role;
        }

        internal void ChangeRole(EmployeeRoleOnProject role)
        {
            Role = role;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Employee;
            yield return Project;
        }
    }

    public enum EmployeeRoleOnProject
    {
        Employee, Manager
    }
}
