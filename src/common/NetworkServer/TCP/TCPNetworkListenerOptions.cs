using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mir.Network.TCP
{
    public class TCPNetworkListenerOptions
    {
        public IPAddress ListenIP { get; set; } = IPAddress.Any;
        public ushort ListenPort { get; set; } = 3000;
        public PacketSource Source { get; set; } 
    }
}
