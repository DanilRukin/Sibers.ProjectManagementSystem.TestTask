using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.Configurations
{
    public class EmployeeAgregateConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(DataConstants.EMPLOYEES_TABLE_NAME);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.OwnsOne(e => e.Email);
            builder.OwnsOne(e => e.PersonalData);

            builder.Ignore(e => e.DomainEvents);
            builder.Ignore(e => e.OnTheseProjectsIsManager);
            builder.Ignore(e => e.OnTheseProjectsIsEmployee);
            builder.Ignore(e => e.Projects);

            builder.HasMany<EmployeeOnProject>(DataConstants.EMPLOYEE_ON_PROJECTS)
                .WithOne(eop => eop.Employee)
                .HasForeignKey(eop => eop.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(DataConstants.EMPLOYEE_ON_PROJECTS)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany<Domain.TaskEntity.Task>(DataConstants.CREATED_TASKS)
                .WithOne()
                .HasForeignKey(t => t.AuthorEmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<Domain.TaskEntity.Task>(DataConstants.EXECUTABLE_TASKS)
                .WithOne()
                .HasForeignKey(t => t.ContractorEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
