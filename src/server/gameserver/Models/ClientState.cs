using Mir.Packets;
using Mir.Packets.Gate;
using Mir.Packets.Server;
using System;
using System.Threading.Tasks;

namespace Mir.GameServer.Models
{
    public enum Stage : byte
    {
        Login = 0,
        Characters = 1,
        Game = 2
    }

    public class ClientState
    {
        public ClientState(GateConnection gateConnection, int socketHandle)
        {
            GateConnection = gateConnection;
            SocketHandle = socketHandle;
        }

        public Stage Stage { get; set; } = Stage.Login;
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

        public async Task Disconnect(string reason)
        {
            GateConnection.Clients.TryRemove(SocketHandle, out ClientState s);
            await Send(new Disconnect { Reason = reason });
        }
    }
}
