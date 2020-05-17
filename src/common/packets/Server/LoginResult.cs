using Mir.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Server
{
    public enum LoginResultEnum : byte
    {
        Succcess = 1,
        BadUsernameOrPassword = 2,
        BlockedIP = 3,

    }

    [ProtoContract]
    public class LoginResult : Packet
    {
        [ProtoMember(1)]
        public LoginResultEnum Result { get; set; }

        [ProtoMember(2)]
        public IEnumerable<Character> Characters { get; set; }
    }
}
