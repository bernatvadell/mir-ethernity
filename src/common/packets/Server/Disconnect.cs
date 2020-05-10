using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Server
{
    [ProtoContract]
    public class Disconnect : Packet
    {
        [ProtoMember(1)]
        public string Reason { get; set; }
    }
}
