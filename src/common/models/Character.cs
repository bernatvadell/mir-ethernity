using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Models
{
    [ProtoContract]
    public class Character
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public ushort Level { get; set; }
        [ProtoMember(4)]
        public MirClass Class { get; set; }
        [ProtoMember(5)]
        public MirGender Gender { get; set; }
    }
}
