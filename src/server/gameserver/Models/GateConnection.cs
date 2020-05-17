using Mir.Network;
using System.Collections.Generic;

namespace Mir.GameServer.Models
{
    public class GateConnection
	{
		public IConnection Connection { get; }

		public Dictionary<int, ClientState> Clients { get; } = new Dictionary<int, ClientState>();
		
		public GateConnection(IConnection connection)
		{
			Connection = connection;
		}
	}
}
