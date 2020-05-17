using Mir.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mir.GameServer.Models
{
    public class GameState
	{
		public TimeSpan GameTime { get; set; } = TimeSpan.Zero;
		public Dictionary<int, GateConnection> Gates { get; } = new Dictionary<int, GateConnection>();
		
		public ConcurrentQueue<GateConnection> ConnectingGates { get; } = new ConcurrentQueue<GateConnection>();
		public ConcurrentQueue<GateConnection> DisconnectedGates { get; } = new ConcurrentQueue<GateConnection>();
		public ConcurrentQueue<Message> GateMessages { get; } = new ConcurrentQueue<Message>();
		public ConcurrentQueue<ClientState> DisconnectingClients { get; } = new ConcurrentQueue<ClientState>();
		public ConcurrentQueue<ClientState> ConnectingClients { get; } = new ConcurrentQueue<ClientState>();
	}
}
