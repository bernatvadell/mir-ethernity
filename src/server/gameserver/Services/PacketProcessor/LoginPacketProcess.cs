using Mir.Network;
using Mir.Packets.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.PacketProcessor
{
	public class LoginPacketProcess : PacketProcess<Login>
	{
		public override async Task Process(IConnection connection, Login packet)
		{
			
		}
	}
}
