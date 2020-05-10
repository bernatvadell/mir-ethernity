using Microsoft.Extensions.Logging;
using Mir.Network;
using Mir.Packets.Gate;
using Mir.Packets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services.Default
{
    public class GateService : IService
    {
        private readonly ILogger<GateService> _logger;
        private readonly IClient _serverClient;
        private readonly IListener _listener;
        private CancellationToken _cancellationToken;

        public GateService(IListener listener, IClient serverClient, ILogger<GateService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _serverClient = serverClient ?? throw new ArgumentNullException(nameof(serverClient));
            _serverClient.OnDisconnect += ServerClient_OnDisconnect;
            _serverClient.OnData += ServerClient_OnData;

            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _listener.OnClientConnect += Listener_OnClientConnect;
            _listener.OnClientDisconnect += Listener_OnClientDisconnect;
            _listener.OnClientData += Listener_OnClientData;
        }

        private void ServerClient_OnData(object sender, Packets.Packet e)
        {

        }

        private async void ServerClient_OnDisconnect(object sender, EventArgs e)
        {
            var connections = _listener.Connections.ToArray();
            foreach (var con in connections)
            {
                await con.Send(new Disconnect() { Reason = "Server is down" });
                await con.Disconnect();
            }

            _logger.LogInformation($"Game server disconnected");

            TryConnectToGameServer();
        }

        private void Listener_OnClientData(object sender, Message e)
        {
            _serverClient.Send(new ClientPacket { SocketHandle = e.Connection.Handle, Packet = e.Packet });
        }

        private async void Listener_OnClientDisconnect(object sender, IConnection e)
        {
            await _serverClient.Send(new ClientConnectionChanged() { Connected = false, SocketHandle = e.Handle });
        }

        private async void Listener_OnClientConnect(object sender, IConnection e)
        {
            if (!_serverClient.Connected)
            {
                await e.Send(new Disconnect() { Reason = "Server is down" });
                await e.Disconnect();
            }

            await _serverClient.Send(new ClientConnectionChanged() { Connected = true, SocketHandle = e.Handle });
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;

            TryConnectToGameServer();

            await _listener.Listen(cancellationToken);
        }

        public async void TryConnectToGameServer()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Trying to connect to game server");
                try
                {
                    await _serverClient.Connect(_cancellationToken);
                    _logger.LogInformation($"Connected to game server");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Cant connect to game server, next try in 3 seconds...");
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
