using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
            throw new NotImplementedException();
        }
    }
}
