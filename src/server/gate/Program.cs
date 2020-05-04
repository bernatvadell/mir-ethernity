using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GateServer.Exceptions;
using Mir.GateServer.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Env.Load();

                var container = IoCBuilder.BuildContainer();
                var service = container.Resolve<IService>();
                var logger = container.Resolve<ILogger<Program>>();
                var cts = new CancellationTokenSource();

                Console.CancelKeyPress += (s, e) =>
                {
                    logger.LogInformation("Signal cancel received, wait to end service...");
                    cts.Cancel();
                };

                try
                {
                    await service.Run(cts.Token);
                    logger.LogInformation("Server stopped successfully");
                }
                catch (BadConfigValueException ex)
                {
                    logger.LogError(ex, "Bad format config");
                }
            }
            catch (TaskCanceledException)
            {
                Environment.Exit(0);
            }
        }
    }
}
