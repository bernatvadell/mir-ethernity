using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Common
{
    [Packet(PacketSource.Common, PacketIndex.Ping)]
    [ProtoContract]
    public class Ping : Packet { }
}
