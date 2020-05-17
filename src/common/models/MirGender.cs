using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Models
{
    [ProtoContract]
    public class MirGender
    {
        [ProtoMember(1)]
        public byte Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
