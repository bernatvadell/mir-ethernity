using Autofac;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using Mir.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.Network.TCP
{
    public class TCPNetworkListener : IListener
    {
        private Socket _listenerSocket;
        private readonly SemaphoreSlim _waiterConnection = new SemaphoreSlim(0);

        private ILifetimeScope _container;
        private ILogger<TCPNetworkListener> _logger;
        private TCPNetworkListenerOptions _options;
        private ConcurrentDictionary<IntPtr, TCPConnection> _connections;

        public event EventHandler<IConnection> OnClientConnect;
        public event EventHandler<IConnection> OnClientDisconnect;
        public event EventHandler<Message> OnClientData;

        public IEnumerable<IConnection> Connections => _connections.Values.Cast<IConnection>();

        public bool Listening { get; private set; }

        public TCPNetworkListener(ILifetimeScope container, ILogger<TCPNetworkListener> logger, TCPNetworkListenerOptions options)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _connections = new ConcurrentDictionary<IntPtr, TCPConnection>();
        }

        public async Task Listen(CancellationToken cancellationToken = default)
        {
            if (Listening) return;

            _listenerSocket = new Socket(_options.ListenIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listenerSocket.Bind(new IPEndPoint(_options.ListenIP, _options.ListenPort));
                _listenerSocket.Listen(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred starting listening");
                return;
            }


            Listening = true;

            _logger.LogInformation($"Network started: {_options.ListenIP}:{_options.ListenPort}");

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

        private void AcceptCallback(IAsyncResult ar)
        {
            CancellationToken cancellationToken = (CancellationToken)ar.AsyncState;
            _waiterConnection.Release();
            Socket handler = _listenerSocket.EndAccept(ar);

            _logger.LogDebug($"Client connected - Handle: " + handler.Handle);
            var connection = _container.Resolve<TCPConnection>(new TypedParameter(typeof(Socket), handler));
            _connections.TryAdd(handler.Handle, connection);

            connection.OnReceivePacket += (s, e) => OnClientData?.Invoke(this, new Message((IConnection)s, e));
            connection.OnDisconnect += (s, e) => OnClientDisconnect?.Invoke(this, (IConnection)s);

            OnClientConnect?.Invoke(this, connection);
            connection.StartListenData(cancellationToken, _options.Source, true);
        }
    }
}
