using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data.DataProfiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.DataProfiles
{
    public class PostgresDatabaseProfile : DataProfile
    {
        public string MigrationAssembly { get; set; }
        public PostgresDatabaseProfile(string name, string connectionString, bool useSeedData, bool migrateDatabase, bool createDatabase, string migrationAssembly) :
            base(name, connectionString, useSeedData, migrateDatabase, createDatabase)
        {
            MigrationAssembly = migrationAssembly;
        }

        public override void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(ConnectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        }
    }
}
