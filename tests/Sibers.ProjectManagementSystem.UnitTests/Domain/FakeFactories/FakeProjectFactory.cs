using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain.FakeFactories
{
    internal class FakeProjectFactory : IProjectFactory
    {
        private int _nextId = 1;
        public Project Create(string name, DateTime startDate, DateTime endDate, Priority priority,
            string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            Project project = new Project(name, startDate, endDate, priority, nameOfTheCustomerCompany,
                nameOfTheContractorComapny);
            PropertyInfo? property = project.GetType().GetProperty(nameof(Project.Id));
            if (property != null )
            {
                property.SetValue(project, _nextId);
                _nextId++;
            }
            return project;
        }
    }
}
