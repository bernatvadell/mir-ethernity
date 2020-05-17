using Mir.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mir.GameServer.Models
{
    public class GameState
	{
		public TimeSpan GameTime { get; set; } = TimeSpan.Zero;
		public ConcurrentDictionary<int, GateConnection> Gates { get; } = new ConcurrentDictionary<int, GateConnection>();
		public ConcurrentQueue<Message> IncommingPackets { get; } = new ConcurrentQueue<Message>();

		//public ConcurrentQueue<GateConnection> ConnectingGates { get; } = new ConcurrentQueue<GateConnection>();
		//public ConcurrentQueue<GateConnection> DisconnectedGates { get; } = new ConcurrentQueue<GateConnection>();
		//public ConcurrentQueue<ClientState> DisconnectingClients { get; } = new ConcurrentQueue<ClientState>();
		//public ConcurrentQueue<ClientState> ConnectingClients { get; } = new ConcurrentQueue<ClientState>();
	}
}
