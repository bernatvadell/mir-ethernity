using Microsoft.Extensions.Logging;
using Mir.GameServer.Services.LoopTasks;
using Mir.GameServer.Services.PacketProcessor;
using Mir.Network;
using Mir.Packets.Gate;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.Default
{
	public class ClientState
	{
		public ClientState(GateConnection gateConnection, int socketHandle)
		{
			GateConnection = gateConnection;
			SocketHandle = socketHandle;
		}

		public GateConnection GateConnection { get; }
		public int SocketHandle { get; }
	}

	public class GateConnection
	{
		public IConnection Connection { get; }

		public Dictionary<int, ClientState> Clients { get; } = new Dictionary<int, ClientState>();
		
		public GateConnection(IConnection connection)
		{
			Connection = connection;
		}
	}

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

	public class GameService : IService
	{
		private readonly ILogger<GameService> _logger;
		private readonly IDictionary<int, ILoopTask[]> _loopTasks;
		private readonly GameState _state;
		private readonly IListener _listener;
		private readonly PacketProcessExecutor _packetProcessExecutor;
		public ConcurrentDictionary<int, GateConnection> Gates { get; } = new ConcurrentDictionary<int, GateConnection>();

		public GameService(
			IListener listener,
			ILogger<GameService> logger,
			GameState state,
			PacketProcessExecutor packetProcessExecutor,
			IEnumerable<ILoopTask> loopTasks
		)
		{
			_logger = logger;
			_loopTasks = loopTasks.GroupBy(x => x.Order)?.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.ToArray()) ?? throw new ArgumentNullException(nameof(LoopTasks));
			_state = state ?? throw new ArgumentNullException(nameof(state));
			_listener = listener ?? throw new ArgumentNullException(nameof(listener));
			_packetProcessExecutor = packetProcessExecutor ?? throw new ArgumentNullException(nameof(packetProcessExecutor));

			_listener.OnClientConnect += _listener_OnClientConnect;
			_listener.OnClientData += _listener_OnClientData;
			_listener.OnClientDisconnect += _listener_OnClientDisconnect;
		}

		private void _listener_OnClientData(object sender, Message e)
		{
			_state.GateMessages.Enqueue(e);
		}

		private void _listener_OnClientDisconnect(object sender, IConnection e)
		{
			if (_state.Gates.ContainsKey(e.Handle))
				_state.DisconnectedGates.Enqueue(_state.Gates[e.Handle]);
		}

		private void _listener_OnClientConnect(object sender, IConnection e)
		{
			var gate = new GateConnection(e);
			_state.ConnectingGates.Enqueue(gate);
		}

		public async Task Run(CancellationToken cancellationToken = default)
		{
			await Task.WhenAny(
				Update(cancellationToken),
				_listener.Listen(cancellationToken)
			);
		}

		private async Task Update(CancellationToken cancellationToken)
		{
			var sw = new Stopwatch();

			sw.Start();

			var fpsExpected = 120;
			var msPerUpdate = 1000 / fpsExpected;

			while (!cancellationToken.IsCancellationRequested)
			{
				_state.GameTime = sw.Elapsed;

				for (var i = 0; i < _loopTasks.Count; i++)
				{
					var group = _loopTasks[i];
					var tasks = new Task[group.Length];
					for (var t = 0; t < tasks.Length; t++)
						tasks[t] = group[t].Update(_state);
					await Task.WhenAll(tasks);
				}

				var elapsedMiliseconds = (sw.Elapsed - _state.GameTime).TotalMilliseconds;

				if (msPerUpdate > elapsedMiliseconds)
					await Task.Delay(TimeSpan.FromMilliseconds(msPerUpdate - elapsedMiliseconds));
			}

			sw.Stop();
		}
	}
}
