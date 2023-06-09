using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain;

namespace Sibers.ProjectManagementSystem.Data
{
    public class ProjectManagementSystemContext : DbContext, IEmployeeFactory, IProjectFactory
    {
        IDomainEventDispatcher _domainEventDispatcher;
        public ProjectManagementSystemContext(IDomainEventDispatcher domainEventDispatcher, DbContextOptions<ProjectManagementSystemContext> options) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeOnProject> EmployeesOnProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            var events = ChangeTracker.Entries<IDomainObject>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();
            await _domainEventDispatcher.DispatchAndClearEvents(events);
        }

        public Project Create(string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            Project project = new Project(name, startDate, endDate, priority, nameOfTheCustomerCompany, nameOfTheContractorComapny);
            Projects.Add(project);
            SaveEntitiesAsync(new CancellationToken());
            return project;
        }

        public Employee Create(PersonalData personalData, Email email)
        {
            Employee employee = new Employee(personalData, email);
            Employees.Add(employee);
            SaveEntitiesAsync(new CancellationToken());
            return employee;
        }
    }
}
