using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sibers.ProjectManagementSystem.Api;
using Sibers.ProjectManagementSystem.Application;
using Sibers.ProjectManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProjectManagementSystemContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                services.AddDbContext<ProjectManagementSystemContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                IServiceProvider serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    var context = serviceProvider.GetRequiredService<ProjectManagementSystemContext>();
                    var logger = serviceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    try
                    {
                        SeedData.InitializeDatabase(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                "database with test data. Error: {exceptionMessage}", ex.Message);
                    }
                }
            });
        }

        public void ResetDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                IServiceProvider scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ProjectManagementSystemContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                try
                {
                    SeedData.ResetDatabase(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                            "database with test data. Error: {exceptionMessage}", ex.Message);
                }
            }
        }
    }
}
