using Mir.Packets;
using Mir.Packets.Gate;
using System;
using System.Threading.Tasks;

namespace Mir.GameServer.Models
{
    public class ClientState
    {
        public ClientState(GameState state, GateConnection gateConnection, int socketHandle)
        {
            GameState = state;
            GateConnection = gateConnection;
            SocketHandle = socketHandle;
        }

        public GameState GameState { get; }
        public GateConnection GateConnection { get; }
        public int SocketHandle { get; }

        public async Task Send(Packet packet)
        {
            await GateConnection.Connection.Send(new ClientPacket
            {
                SocketHandle = SocketHandle,
                Packet = packet
            });
        }

        public Task Disconnect()
        {
            GateConnection.Clients.TryRemove(SocketHandle, out ClientState s);
            return Task.CompletedTask;
        }
    }
}
