using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Repository.Migratoions;

namespace Repository
{
    public class Migrator
    {
        public static void Execute(string connectionString)
        {
            var serviceProvider = CreateServices(connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                UpdateDatabase(runner);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb.AddPostgres()
                      .WithGlobalConnectionString(connectionString)
                      .ScanIn(typeof(CreateSchemaUser).Assembly).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IMigrationRunner runner, int attemps = 0)
        {
            try
            {
                runner.MigrateUp();
            }
            catch (Exception)
            {
                if (attemps < 3)
                {
                    Thread.Sleep(1000);
                    UpdateDatabase(runner, attemps + 1);
                }
            }
        }
    }
}
