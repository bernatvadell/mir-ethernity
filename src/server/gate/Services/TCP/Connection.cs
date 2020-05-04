using Microsoft.Extensions.Logging;
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
        private byte[] _buffer = new byte[1024];

        public bool Connected { get; private set; } = true;

        public Connection(Socket socket, ILogger<Connection> logger)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            while (Connected && !cancellationToken.IsCancellationRequested)
            {
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), cancellationToken);

                try
                {
                    await _waiterData.WaitAsync(Timeout.Infinite, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public void Disconnect()
        {
            Connected = false;
            _waiterData.Release();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            CancellationToken cancellationToken = (CancellationToken)ar.AsyncState;

            if (cancellationToken.IsCancellationRequested)
                return;

            int length = _socket.EndReceive(ar);

            if (length == 0)
            {
                Disconnect();
                return;
            }

            var data = new byte[length];
            Array.Copy(_buffer, data, length);

            _logger.LogDebug($"[Socket] {_socket.Handle} - Receive data: {Convert.ToBase64String(data)}");

            _socket.BeginSend(Encoding.UTF8.GetBytes("OK"), 0, 2, SocketFlags.None, SendCallback, cancellationToken);

            _waiterData.Release();
        }

        private void SendCallback(IAsyncResult ar)
        {
            CancellationToken cancellationToken = (CancellationToken)ar.AsyncState;

            if (cancellationToken.IsCancellationRequested)
                return;

            int bytesSent = _socket.EndSend(ar);
        }
    }
}
