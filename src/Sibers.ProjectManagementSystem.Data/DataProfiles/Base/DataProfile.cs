using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.DataProfiles.Base
{
    public abstract class DataProfile
    {
        public string Name { get; protected set; }
        public string ConnectionString { get; protected set; }
        public bool UseSeedData { get; protected set; }
        public bool MigrateDatabase { get; protected set; }
        public bool CreateDatabase { get; protected set; }

        protected DataProfile(string name, string connectionString, bool useSeedData,
            bool migrateDatabase, bool createDatabase)
        {
            Name = name;
            ConnectionString = connectionString;
            UseSeedData = useSeedData;
            MigrateDatabase = migrateDatabase;
            CreateDatabase = createDatabase;
        }

        public abstract void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder);

        public virtual void UseDbContext(ProjectManagementSystemContext context)
        {
            if (UseSeedData)
            {
                if (MigrateDatabase)
                {
                    SeedData.ApplyMigrationAndFillDatabase(context);
                }
                else if (CreateDatabase)
                {
                    SeedData.InitializeDatabase(context);
                }
            }
        }
    }
}
