﻿using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data.DataProfiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.DataProfiles
{
    public class InMemoryDatabaseProfile : DataProfile
    {
        public InMemoryDatabaseProfile(string name, string connectionString, bool useSeedData, bool migrateDatabase, bool createDatabase) :
            base(name, connectionString, useSeedData, migrateDatabase, createDatabase)
        {
        }

        public override void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
        {
            builder.UseInMemoryDatabase(ConnectionString);
        }
    }
}
