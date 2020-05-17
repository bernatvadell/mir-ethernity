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
        public static void Execute(ProviderType providerType, string connectionString)
        {
            var serviceProvider = CreateServices(providerType, connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                UpdateDatabase(runner);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices(ProviderType providerType, string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    switch (providerType)
                    {
                        case ProviderType.PostgreSQL:
                            rb.AddPostgres();
                            break;
                        case ProviderType.MySQL:
                            rb.AddMySql5();
                            break;
                        case ProviderType.SqlServer:
                            rb.AddSqlServer();
                            break;
                    }

                    rb.WithGlobalConnectionString(connectionString)
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
