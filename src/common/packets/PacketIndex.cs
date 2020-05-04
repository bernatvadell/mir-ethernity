using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets
{
    public enum PacketIndex : ushort
    {
        Ping = 0,
        Pong = 1,
        
        ClientPacket = 2,
        Disconnect = 3,
        ClientConnectionChanged = 4
    }
}
