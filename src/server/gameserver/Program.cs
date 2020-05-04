using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.GameServer.Exceptions;
using Mir.GameServer.Services;
using Mir.Network.TCP;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer
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

                await service.Run(cts.Token);
                logger.LogInformation("Server stopped successfully");
            }
            catch (TaskCanceledException)
            {
                Environment.Exit(0);
            }
        }
    }
}
