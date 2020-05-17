using Mir.Packets.Client;
using Mir.Packets.Common;
using Mir.Packets.Gate;
using Mir.Packets.Server;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Packets
{
	[ProtoContract]
	#region Common Packets
	[ProtoInclude((int)PacketIndex.Ping, typeof(Ping))]
	[ProtoInclude((int)PacketIndex.Pong, typeof(Pong))]
	[ProtoInclude((int)PacketIndex.Disconnect, typeof(Disconnect))]
	#endregion

	#region Gate Packets
	[ProtoInclude((int)PacketIndex.ClientPacket, typeof(ClientPacket))]
	[ProtoInclude((int)PacketIndex.ClientConnectionChanged, typeof(ClientConnectionChanged))]
	#endregion

	#region Client Packets
	[ProtoInclude((int)PacketIndex.Login, typeof(Login))]
	#endregion

	#region Server Packets
	[ProtoInclude((int)PacketIndex.LoginResult, typeof(LoginResult))]
	#endregion
	public abstract class Packet
	{
	}
}
