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
using Sibers.ProjectManagementSystem.Domain.TaskEntity;

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
        public DbSet<Domain.TaskEntity.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
            builder.Properties<DateTime>()
                .HaveConversion<Converters.DateTimeUtcConverter>();
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

        public async Task<Project> CreateProject(string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            Project project = new Project(name, startDate, endDate, priority, nameOfTheCustomerCompany, nameOfTheContractorComapny);
            Projects.Add(project);
            await SaveEntitiesAsync(new CancellationToken());
            return project;
        }

        Project IProjectFactory.Create(string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            Project project = new Project(name, startDate, endDate, priority, nameOfTheCustomerCompany, nameOfTheContractorComapny);
            Projects.Add(project);
            SaveEntitiesAsync(new CancellationToken()).Wait();
            return project;
        }

        public async Task<Employee> CreateEmployee(PersonalData personalData, Email email)
        {
            Employee employee = new Employee(personalData, email);
            Employees.Add(employee);
            await SaveEntitiesAsync(new CancellationToken());
            return employee;
        }

        Employee IEmployeeFactory.Create(PersonalData personalData, Email email)
        {
            Employee employee = new Employee(personalData, email);
            Employees.Add(employee);
            SaveEntitiesAsync(new CancellationToken()).Wait();
            return employee;
        }
        
    }
}
