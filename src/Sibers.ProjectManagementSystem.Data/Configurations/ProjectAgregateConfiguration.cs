using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.Configurations
{
    public class ProjectAgregateConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(DataConstants.PROJECTS_TABLE_NAME);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Ignore(p => p.DomainEvents);
            builder.Ignore(p => p.Employees);
            builder.Ignore(p => p.Manager);
            builder.Ignore(p => p.Tasks);

            builder.OwnsOne(p => p.Priority);

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.NameOfTheCustomerCompany).IsRequired();
            builder.Property(p => p.NameOfTheContractorCompany).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();

            builder.HasMany<EmployeeOnProject>(DataConstants.EMPLOYEES_ON_PROJECT)
                .WithOne(eop => eop.Project)
                .HasForeignKey(eop => eop.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(DataConstants.EMPLOYEES_ON_PROJECT)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany<Domain.TaskEntity.Task>(DataConstants.PROJECT_HAS_TASKS)
                .WithOne()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
