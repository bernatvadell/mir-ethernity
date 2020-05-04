using Autofac;
using Microsoft.Extensions.Logging;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.Network.TCP
{
    public class TCPNetworkClient : IClient
    {
        private readonly SemaphoreSlim _waiterConnect = new SemaphoreSlim(0);
        private readonly ILogger<TCPNetworkClient> _logger;
        private readonly TCPNetworkClientOptions _options;
        private readonly ILifetimeScope _container;
        private Socket _socket;
        private TCPConnection _connection;
        private Exception _connectException;

        public bool Connected { get => _connection?.Connected ?? false; }

        public event EventHandler OnDisconnect;
        public event EventHandler<Packet> OnData;

        public TCPNetworkClient(TCPNetworkClientOptions options, ILogger<TCPNetworkClient> logger, ILifetimeScope container)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _container = container;
        }

        public async Task Connect(CancellationToken cancellationToken = default)
        {
            if (Connected) throw new ApplicationException("You're connected, first disconnect it");

            _socket?.Dispose();

            _socket = new Socket(_options.ServerIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(_options.ServerIP, _options.ServerPort, new AsyncCallback(ConnectCallback), cancellationToken);

            if (!await _waiterConnect.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken))
                throw new TimeoutException();

            if (_connectException != null)
                throw _connectException;

            _connection.StartListenData(cancellationToken, _options.Source, false);

            _logger.LogDebug($"Connected to server {_options.ServerIP}:{_options.ServerPort}");
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            _connectException = null;

            CancellationToken cancellationToken = (CancellationToken)ar.AsyncState;

            if (cancellationToken.IsCancellationRequested)
                return;

            try
            {
                _socket.EndConnect(ar);
                _connection = _container.Resolve<TCPConnection>(new TypedParameter(typeof(Socket), _socket));
                _connection.OnReceivePacket += Connection_OnReceivePacket;
                _connection.OnDisconnect += Connection_OnDisconnect;
            }
            catch (Exception ex)
            {
                _connectException = ex;
            }
            finally
            {
                _waiterConnect.Release();
            }
        }

        private void Connection_OnDisconnect(object sender, EventArgs e)
        {
            _socket = null;
            _connection = null;
            OnDisconnect?.Invoke(this, e);
        }

        private void Connection_OnReceivePacket(object sender, Packet e)
        {
            OnData?.Invoke(this, e);
        }

        public async Task Send(Packet packet)
        {
            if (!Connected) throw new ApplicationException();
            await _connection.Send(packet);
        }

        public void Disconnect()
        {
            _connection?.Disconnect();
        }
    }
}
