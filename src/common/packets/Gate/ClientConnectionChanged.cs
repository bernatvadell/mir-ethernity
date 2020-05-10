using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Gate
{
    [ProtoContract]
    public class ClientConnectionChanged : Packet
    {
        [ProtoMember(1)]
        public int SocketHandle { get; set; }
        [ProtoMember(2)]
        public bool Connected { get; set; }
    }
}
