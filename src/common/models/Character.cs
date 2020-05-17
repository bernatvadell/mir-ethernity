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
        public int AccountId { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public DateTime CreationDate { get; set; }
        [ProtoMember(5)]
        public MirClass Class { get; set; }
        [ProtoMember(6)]
        public MirGender Gender { get; set; }
        public int HairColor { get; set; }
        [ProtoMember(4)]
        public ushort Level { get; set; }
        [ProtoMember(4)]
        public decimal Experience { get; set; }
        [ProtoMember(4)]
        public int CurrentHP { get; set; }
        [ProtoMember(4)]
        public int CurrentMP { get; set; }
        [ProtoMember(4)]
        public AttackMode AttackMode { get; set; }
    }
}
