using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GameServer.Services;
using Mir.GameServer.Services.Default;
using Mir.GameServer.Services.PacketProcessor;
using Mir.Network;
using Mir.Network.TCP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Mir.GameServer
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
			builder.RegisterType<GameState>().As<IService>().SingleInstance();
			builder.RegisterType<GameService>().As<IService>().SingleInstance();

			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				.AsClosedTypesOf(typeof(PacketProcess<>))
				.SingleInstance();


			return builder.Build();
		}
	}
}
