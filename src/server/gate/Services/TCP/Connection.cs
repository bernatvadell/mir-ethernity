using Microsoft.Extensions.Logging;
using Mir.Packets;
using Mir.Packets.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services.TCP
{
    public class Connection : IConnection
    {
        private readonly SemaphoreSlim _waiterData = new SemaphoreSlim(0);
        private readonly Socket _socket;
        private readonly ILogger<Connection> _logger;
        private byte[] _buffer = new byte[1024 * 16];
        private byte[] _extra = new byte[0];
        private CancellationToken _cancellationToken;

        public bool Connected { get; private set; } = true;

        public Queue<Packet> InconmingPackets = new Queue<Packet>();

        public Connection(Socket socket, ILogger<Connection> logger)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            while (Connected && !cancellationToken.IsCancellationRequested)
            {
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);

                try
                {
                    if (!await _waiterData.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
                    {
                        Send(Ping.Default);

                        if (!await _waiterData.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken))
                            Disconnect();
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public void Send(Packet packet)
        {
            var buffer = PacketSerializer.Serialize(packet);
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        public void Disconnect()
        {
            Connected = false;
            _waiterData.Release();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            if (_cancellationToken.IsCancellationRequested)
                return;

            int length = _socket.EndReceive(ar);

            if (length == 0)
            {
                Disconnect();
                return;
            }

            var data = new byte[length + _extra.Length];
            Array.Copy(_extra, 0, data, 0, _extra.Length);
            Array.Copy(_buffer, 0, data, _extra.Length, length);

            _logger.LogDebug($"[Socket] {_socket.Handle} - Receive data: {Convert.ToBase64String(data)}");

            try
            {
                var packet = PacketSerializer.Deserialize(data, out byte[] _extra, PacketSource.Client);

                if (!(packet is Pong))
                    InconmingPackets.Enqueue(packet);
            }
            catch (WrongPacketException)
            {
                Disconnect();
            }


            _waiterData.Release();
        }

        private void SendCallback(IAsyncResult ar)
        {
            if (_cancellationToken.IsCancellationRequested)
                return;

            _socket.EndSend(ar);
        }
    }
}
