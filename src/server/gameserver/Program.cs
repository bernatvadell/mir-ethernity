using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GameServer.Exceptions;
using Mir.GameServer.Services;
using System;
using System.Threading.Tasks;

namespace Mir.GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Env.Load();

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

            var container = builder.Build();

            var logger = container.Resolve<ILogger<Program>>();
            var listener = container.Resolve<IListener>();

            try
            {
                logger.LogInformation("Starting GameServer");

                await listener.Listen();

                logger.LogInformation("Stopped GameServer");
            }
            catch (BadConfigValueException ex)
            {
                logger.LogError(ex, "Bad format config");
            }
        }
    }
}
