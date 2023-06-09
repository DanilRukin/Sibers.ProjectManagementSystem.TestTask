using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data.DataProfiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.DataProfiles
{
    public class SqlServerDatabaseProfile : DataProfile
    {
        public string MigrationAssembly { get; private set; }
        public SqlServerDatabaseProfile(string name, string connectionString, bool useSeedData, bool migrateDatabase, bool createDatabase, string migrationAssembly) :
            base(name, connectionString, useSeedData, migrateDatabase, createDatabase)
        {
            MigrationAssembly = migrationAssembly;
        }

        public override void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        }
    }
}
