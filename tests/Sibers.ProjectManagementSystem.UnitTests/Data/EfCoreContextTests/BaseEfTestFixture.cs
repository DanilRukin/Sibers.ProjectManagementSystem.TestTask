using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Data.EfCoreContextTests
{
    public abstract class BaseEfTestFixture
    {
        protected ProjectManagementSystemContext GetClearContext()
        {
            var options = CreateDbContextOptions();
            var mockEventDispatcher = new Mock<IDomainEventDispatcher>();

            return new ProjectManagementSystemContext(mockEventDispatcher.Object, options);
        }

        protected static DbContextOptions<ProjectManagementSystemContext> CreateDbContextOptions()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ProjectManagementSystemContext>();
            builder.UseInMemoryDatabase("TestInMemoryDatabase")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
