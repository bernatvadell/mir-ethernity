using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.Network
{
    public interface IConnection
    {
        event EventHandler<Packet> OnReceivePacket;
        event EventHandler OnDisconnect;
        bool Connected { get; }
        int Handle { get; }

        Task Send(Packet packet);
        Task Disconnect();
    }
}
