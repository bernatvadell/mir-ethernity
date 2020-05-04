using Microsoft.Extensions.Logging;
using Mir.Network;
using Mir.Packets.Gate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.Default
{
    public class GameService : IService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IListener _listener;

        public GameService(IListener listener, ILogger<GameService> logger)
        {
            _logger = logger;
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _listener.OnClientConnect += _listener_OnClientConnect;
            _listener.OnClientData += _listener_OnClientData;
            _listener.OnClientDisconnect += _listener_OnClientDisconnect;
        }

        private void _listener_OnClientDisconnect(object sender, IConnection e)
        {
            _logger.LogInformation("Gate Server disconnected");
        }

        private void _listener_OnClientData(object sender, Message e)
        {
            if (e.Packet is ClientConnectionChanged p)
            {
                if (p.Connected)
                {
                    _logger.LogInformation($"Client connected to gate: " + p.SocketHandle);
                }
                else
                {
                    _logger.LogInformation($"Client disconnected to gate: " + p.SocketHandle);
                }
            }
            else
            {
                _logger.LogInformation($"Unknown packet received from gate server");
            }
        }

        private void _listener_OnClientConnect(object sender, IConnection e)
        {
            _logger.LogInformation($"Gate Server connected");
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            await _listener.Listen(cancellationToken);
        }
    }
}
