using Sibers.ProjectManagementSystem.Api.Services;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Data.DataProfiles;
using Sibers.ProjectManagementSystem.Data.DataProfiles.Base;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;

namespace Sibers.ProjectManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfiguration configuration = builder.Configuration;

            IDataProfileFactory dataProfileFactory = new DataProfileFactory(configuration);
            DataProfile dataProfile = dataProfileFactory.CreateProfile();
            // Add services to the container.

            builder.Services
                .AddScoped<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddDbContext<ProjectManagementSystemContext>(dataProfile.ConfigureDbContextOptionsBuilder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ProjectManagementSystemContext>();
                dataProfile.UseDbContext(context);
            }
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}