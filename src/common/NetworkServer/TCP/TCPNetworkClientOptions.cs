using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mir.Network.TCP
{
    public class TCPNetworkClientOptions
    {
        public IPAddress ServerIP { get; set; }
        public ushort ServerPort { get; set; }
        public PacketSource Source { get; set; }
    }
}
