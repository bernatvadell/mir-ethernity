using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GameServer.Models;
using Mir.GameServer.Services;
using Mir.GameServer.Services.Default;
using Mir.GameServer.Services.LoopTasks;
using Mir.GameServer.Services.PacketProcessor;
using Mir.Network;
using Mir.Network.TCP;
using MySql.Data.MySqlClient;
using Npgsql;
using Repository;
using Repository.SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;

namespace Mir.GameServer
{
    public static class IoCBuilder
    {
        private static void RegisterNetworkListener(ContainerBuilder builder)
        {
            builder.Register((c) =>
            {
                var tcpIP = Env.GetString("GS_IP", "0.0.0.0");
                var tcpPORT = Env.GetString("GS_PORT", "7000");

                if (!IPAddress.TryParse(tcpIP, out IPAddress address))
                    throw new ApplicationException($"Invalid config value for GATE_IP");

                if (!ushort.TryParse(tcpPORT, out ushort port) || port == 0 || port > ushort.MaxValue)
                    throw new ApplicationException($"Invalid config value for GATE_PORT");

                return new TCPNetworkListenerOptions { ListenIP = address, ListenPort = port, Source = Packets.PacketSource.Gate };
            }).SingleInstance();

            builder.RegisterType<TCPConnection>().InstancePerDependency();
            builder.RegisterType<TCPNetworkListener>().As<IListener>().SingleInstance();
        }

        private static void RegisterDBProvider(ContainerBuilder builder)
        {
            var providerTypeStr = Env.GetString("DB_PROVIDER", "PostgreSQL");
            var host = Env.GetString("DB_HOST");
            var user = Env.GetString("DB_USER");
            var pass = Env.GetString("DB_PASS");
            var port = Env.GetInt("DB_PORT");
            var db = Env.GetString("DB_DATABASE");
            var connectionString = string.Empty;

            if (!Enum.TryParse(providerTypeStr, out ProviderType providerType))
                throw new ApplicationException($"Unknown DB_PROVIDER type");

            switch (providerType)
            {
                case ProviderType.PostgreSQL:
                    connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={db}";
                    builder.RegisterType<PostgresCompiler>().As<Compiler>().SingleInstance();
                    builder.Register((c) => new NpgsqlConnection(connectionString))
                        .As<IDbConnection>().SingleInstance();
                    break;
                case ProviderType.SqlServer:
                    connectionString = $"User ID={user};Password={pass};Initial Catalog={db};Server={host};Port={port}";
                    builder.RegisterType<SqlServerCompiler>().As<Compiler>().SingleInstance();
                    builder.Register((c) => new SqlConnection(connectionString))
                        .As<IDbConnection>().SingleInstance();
                    break;
                case ProviderType.MySQL:
                    connectionString = $"database={0};server={host};port={port};user id={user};Password={pass}";
                    builder.RegisterType<MySqlCompiler>().As<Compiler>().SingleInstance();
                    builder.Register((c) => new MySqlConnection(connectionString))
                        .As<IDbConnection>().SingleInstance();
                    break;
                default:
                    throw new NotImplementedException();
            }

            builder.RegisterInstance(new DatabaseOptions
            {
                ConnectionString = connectionString,
                Provider = providerType
            });

            builder.RegisterType<QueryFactory>();
        }

        private static void RegisterLogger(ContainerBuilder builder)
        {
            var loggerFactory = LoggerFactory.Create((configure) =>
            {
                configure
                    .SetMinimumLevel(Enum.Parse<LogLevel>(Env.GetString("LOG_LEVEL", "Debug")))
                    .AddConsole();
            });

            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }

        private static void RegisterPacketProcessor(ContainerBuilder builder)
        {
            builder.RegisterType<PacketProcessExecutor>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(PacketProcess<>))
                .SingleInstance();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<GameState>().SingleInstance();
            builder.RegisterType<GameService>().As<IService>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                 .AssignableTo<ILoopTask>()
                 .As<ILoopTask>()
                 .SingleInstance();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();
            builder.RegisterType<CharacterRepository>().As<ICharacterRepository>().SingleInstance();
        }

        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            RegisterLogger(builder);

            RegisterDBProvider(builder);

            RegisterNetworkListener(builder);

            RegisterPacketProcessor(builder);

            RegisterServices(builder);

            RegisterRepositories(builder);

            return builder.Build();
        }
    }
}
