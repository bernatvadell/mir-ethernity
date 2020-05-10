using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets.Client
{
	[ProtoContract]
	public class Login : Packet
	{
		[ProtoMember(1)]
		public string Username { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }
	}
}
