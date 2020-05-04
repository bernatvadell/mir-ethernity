using ProtoBuf;

namespace Mir.Packets.Common
{
    [Packet(PacketSource.Common, PacketIndex.Pong)]
    [ProtoContract]
    public class Pong : Packet { }
}
