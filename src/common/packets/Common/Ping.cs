using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Common
{
    [ProtoContract]
    public class Ping : Packet
    {
        public static Ping Default = new Ping();
    }
}
