using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GateServer.Services;
using Mir.GateServer.Services.Default;
using System;
using System.Collections.Generic;
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

            builder.RegisterType<Services.TCP.Connection>().InstancePerDependency();
            builder.RegisterType<Services.TCP.Listener>().As<IListener>().SingleInstance();

            builder.RegisterType<GateService>().As<IService>().SingleInstance();

            return builder.Build();
        }
    }
}
