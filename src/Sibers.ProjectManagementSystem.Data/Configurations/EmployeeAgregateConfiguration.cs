using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
            throw new NotImplementedException();
        }
    }
}
