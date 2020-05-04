using Autofac;
using EasyTcp.Server;
using Microsoft.Extensions.Logging;
using Mir.GateServer.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services.TCP
{
    public class Listener : IListener
    {
        private Socket _listenerSocket;
        private readonly SemaphoreSlim _waiterConnection = new SemaphoreSlim(0);
        private readonly IPAddress _ipAddress;
        private readonly ushort _port;

        private ILifetimeScope _container;
        private ILogger<Listener> _logger;

        private ConcurrentDictionary<IntPtr, Connection> _connections;

        public bool Listening { get; private set; }

        public Listener(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _logger = container.Resolve<ILogger<Listener>>();

            var tcpIP = DotNetEnv.Env.GetString("GATE_IP", "0.0.0.0");
            var tcpPORT = DotNetEnv.Env.GetString("GATE_PORT", "7000");

            if (!IPAddress.TryParse(tcpIP, out IPAddress address))
                throw new BadConfigValueException("GATE_IP", tcpIP, "IP");

            if (!ushort.TryParse(tcpPORT, out ushort port) || port == 0 || port > ushort.MaxValue)
                throw new BadConfigValueException("GATE_PORT", tcpIP, $"Port number between 1-{ushort.MaxValue}");

            _ipAddress = address;
            _port = port;

            _connections = new ConcurrentDictionary<IntPtr, Connection>();
        }

        public async Task Listen(CancellationToken cancellationToken = default)
        {
            if (Listening) return;

            _listenerSocket = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listenerSocket.Bind(new IPEndPoint(_ipAddress, _port));
                _listenerSocket.Listen(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred starting listening");
                return;
            }


            Listening = true;

            _logger.LogInformation($"Network started: {_ipAddress}:{_port}");

            while (Listening && !cancellationToken.IsCancellationRequested)
            {
                _listenerSocket.BeginAccept(new AsyncCallback(AcceptCallback), cancellationToken);

                try
                {
                    await _waiterConnection.WaitAsync(Timeout.Infinite, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

            }

            Listening = false;

            _logger.LogInformation($"Network stopped");
        }

        private async void AcceptCallback(IAsyncResult ar)
        {
            CancellationToken cancellationToken = (CancellationToken)ar.AsyncState;
            _waiterConnection.Release();
            Socket handler = _listenerSocket.EndAccept(ar);

            _logger.LogDebug($"Client connected - Handle: " + handler.Handle);
            var connection = _container.Resolve<Connection>(new TypedParameter(typeof(Socket), handler));
            _connections.TryAdd(handler.Handle, connection);

            await connection.Initialize(cancellationToken);

            _logger.LogDebug($"Client disconencted - Handle: " + handler.Handle);
        }

    }
}
