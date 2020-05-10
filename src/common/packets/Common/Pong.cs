using ProtoBuf;

namespace Mir.Packets.Common
{
    [ProtoContract]
    public class Pong : Packet
    {
        public static Pong Default = new Pong();
    }
}
