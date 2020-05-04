using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Gate
{
    [Packet(PacketSource.Gate, PacketIndex.ClientPacket)]
    [ProtoContract]
    public class ClientPacket : Packet
    {
        [ProtoMember(1)]
        public int SocketHandle { get; set; }

        [ProtoMember(2)]
        public byte[] Packet { get; set; }
    }
}
