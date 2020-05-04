using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets
{

    public class PacketAttribute : Attribute
    {
        public PacketSource Source { get; }
        public PacketIndex Index { get; }

        public PacketAttribute(PacketSource source, PacketIndex index)
        {
            Index = index;
            Source = source;
        }
    }
}
