using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskEntity.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskEntity.TaskStatus;

namespace Sibers.ProjectManagementSystem.Data.Configurations
{
    internal class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable(DataConstants.TASKS_TABLE_NAME);

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Ignore(t => t.DomainEvents);

            builder.Property(t => t.TaskStatus)
                .HasColumnName(nameof(TaskStatus))
                .HasDefaultValue(TaskStatus.ToDo)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.Name).IsRequired();

            builder.Property(t => t.Description).IsRequired(false);

            builder.OwnsOne(t => t.Priority);

            builder.Property(t => t.ProjectId).IsRequired();

            builder.Property(t => t.AuthorEmployeeId).IsRequired();
            builder.HasOne<Employee>()
                .WithMany(DataConstants.CREATED_TASKS)
                .HasForeignKey(t => t.AuthorEmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(t => t.ContractorEmployeeId).IsRequired(false);
            builder.HasOne<Employee>()
                .WithMany(DataConstants.EXECUTABLE_TASKS)
                .HasForeignKey(t => t.ContractorEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
