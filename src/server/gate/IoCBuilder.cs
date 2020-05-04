using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GateServer.Services;
using Mir.GateServer.Services.Default;
using Mir.Network;
using Mir.Network.TCP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mir.GateServer
{
    public static class IoCBuilder
    {
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
                var tcpIP = Env.GetString("GATE_IP", "0.0.0.0");
                var tcpPORT = Env.GetString("GATE_PORT", "7000");

                if (!IPAddress.TryParse(tcpIP, out IPAddress address))
                    throw new ApplicationException($"Invalid config value for GATE_IP");

                if (!ushort.TryParse(tcpPORT, out ushort port) || port == 0 || port > ushort.MaxValue)
                    throw new ApplicationException($"Invalid config value for GATE_PORT");

                return new TCPNetworkListenerOptions { ListenIP = address, ListenPort = port, Source = Packets.PacketSource.Client };
            }).SingleInstance();

            builder.Register((c) =>
            {
                var tcpIP = Env.GetString("GS_IP", "127.0.0.1");
                var tcpPORT = Env.GetString("GS_PORT", "5000");

                if (!IPAddress.TryParse(tcpIP, out IPAddress address))
                    throw new ApplicationException($"Invalid config value for GS_IP");

                if (!ushort.TryParse(tcpPORT, out ushort port) || port == 0 || port > ushort.MaxValue)
                    throw new ApplicationException($"Invalid config value for GS_PORT");

                return new TCPNetworkClientOptions { ServerIP = address, ServerPort = port, Source = Packets.PacketSource.Server };
            }).SingleInstance();

            builder.RegisterType<TCPConnection>().InstancePerDependency();
            builder.RegisterType<TCPNetworkClient>().As<IClient>().SingleInstance();
            builder.RegisterType<TCPNetworkListener>().As<IListener>().SingleInstance();

            builder.RegisterType<GateService>().As<IService>().SingleInstance();

            return builder.Build();
        }
    }
}
