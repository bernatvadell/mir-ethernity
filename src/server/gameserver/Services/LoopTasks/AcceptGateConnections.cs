using Mir.GameServer.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
	public class AcceptGateConnections : ILoopTask
	{
		public int Order => 0;

		public Task Update(GameState state)
		{
			while (state.ConnectingGates.TryDequeue(out GateConnection gate))
				state.Gates.Add(gate.Connection.Handle, gate);

			return Task.CompletedTask;
		}
	}
}
