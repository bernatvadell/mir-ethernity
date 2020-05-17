using Mir.GameServer.Models;
using Mir.Network;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.PacketProcessor
{
	public abstract class PacketProcess<TPacket> where TPacket : Packet
	{
		public abstract Task Process(ClientState client, TPacket packet);

	}
}
