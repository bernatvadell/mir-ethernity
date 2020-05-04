using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Mir.GameServer.Services.TCP
{
    public class Connection : IConnection
    {
        private readonly TcpClient _client;
        private readonly ILogger<Connection> _logger;

        private NetworkStream _stream;
        private CancellationTokenSource _cts;

        public Connection(TcpClient client, ILogger<Connection> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _cts = new CancellationTokenSource();
            _startReadStream();
        }

        public void Disconnect()
        {
            _client.Close();
            _cts.Cancel();
        }

        private async void _startReadStream()
        {
            _stream = _client.GetStream();

            _logger.LogDebug($"[{_client.Client.Handle}] Starting to read stream");

            while (!_cts.IsCancellationRequested && _client.Connected)
            {
                var buffer = new byte[1024 * 16];
                var length = await _stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);

                if (length == 0)
                {
                    Disconnect();
                    break;
                }

                Array.Resize(ref buffer, length);
                _logger.LogDebug($"[{_client.Client.Handle}] Buffer received: {Convert.ToBase64String(buffer)}");
            }

            _logger.LogDebug($"[{_client.Client.Handle}] End read stream");
        }
    }
}
