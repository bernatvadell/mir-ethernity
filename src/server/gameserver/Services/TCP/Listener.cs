using Autofac;
using Microsoft.Extensions.Logging;
using Mir.GameServer.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.TCP
{
    public class Listener : IListener
    {
        private IPEndPoint _endpoint;
        private TcpListener _listener;
        private ConcurrentBag<Connection> _connections;
        private ILifetimeScope _container;
        private ILogger<Listener> _logger;

        public bool Listening { get; private set; }
        public IReadOnlyCollection<IConnection> Connections { get => _connections; }

        public Listener(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _logger = container.Resolve<ILogger<Listener>>();

            var tcpIP = DotNetEnv.Env.GetString("TCP_IP", "127.0.0.1");
            var tcpPORT = DotNetEnv.Env.GetString("TCP_PORT", "5000");

            if (!IPAddress.TryParse(tcpIP, out IPAddress address))
                throw new BadConfigValueException("TCP_IP", tcpIP, "IP");

            if (!ushort.TryParse(tcpPORT, out ushort port) || port == 0 || port > ushort.MaxValue)
                throw new BadConfigValueException("TCP_PORT", tcpIP, $"Port number between 1-{ushort.MaxValue}");

            _endpoint = new IPEndPoint(address, port);
            _connections = new ConcurrentBag<Connection>();
        }

        public async Task Listen(CancellationToken cancellationToken = default)
        {
            if (Listening) return;

            _listener = new TcpListener(_endpoint);
            _listener.Start();

            Listening = true;

            _logger.LogInformation($"Network started: {_endpoint.Address}:{_endpoint.Port}");

            while (Listening && !cancellationToken.IsCancellationRequested)
            {
                var client = await Task.Run(() => _listener.AcceptTcpClientAsync(), cancellationToken);
                var connection = _container.Resolve<Connection>(new TypedParameter(typeof(TcpClient), client));
                _connections.Add(connection);
            }

            _logger.LogInformation($"Network stopped");
        }
    }
}
