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
using Npgsql;
using Repository;
using Repository.PGSQL;
using System;
using System.Data.Common;
using System.Net;
using System.Reflection;

namespace Mir.GameServer
{
    public static class IoCBuilder
    {
        public static string PostgreSQLConnectionString = $"Host={Env.GetString("PG_HOST")};Port={Env.GetString("PG_PORT")};Username={Env.GetString("PG_USER")};Password={Env.GetString("PG_PASS")};Database={Env.GetString("PG_DB")}";

        public static IContainer BuildContainer()
        {
            var loggerFactory = LoggerFactory.Create((configure) =>
            {
                configure
                    .SetMinimumLevel(Enum.Parse<LogLevel>(Env.GetString("LOG_LEVEL", "Debug")))
                    .AddConsole();
            });

            var builder = new ContainerBuilder();

            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.Register((c) =>
            {
                return new NpgsqlConnection(PostgreSQLConnectionString);
            }).As<NpgsqlConnection>().As<DbConnection>().SingleInstance();

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

            builder.RegisterType<PacketProcessExecutor>().SingleInstance();
            builder.RegisterType<TCPConnection>().InstancePerDependency();
            builder.RegisterType<TCPNetworkListener>().As<IListener>().SingleInstance();
            builder.RegisterType<GameState>().SingleInstance();
            builder.RegisterType<GameService>().As<IService>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                 .AssignableTo<ILoopTask>()
                 .As<ILoopTask>()
                 .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(PacketProcess<>))
                .SingleInstance();


            builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();

            return builder.Build();
        }
    }
}
