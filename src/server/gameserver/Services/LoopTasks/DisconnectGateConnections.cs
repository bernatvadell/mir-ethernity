using Mir.GameServer.Models;
using Mir.GameServer.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
	public class DisconnectGateConnections : ILoopTask
	{
		public int Order => 0;

		public async Task Update(GameState state)
		{
			while (state.DisconnectedGates.TryDequeue(out GateConnection gate))
			{
				state.Gates.Remove(gate.Connection.Handle);

				await gate.Connection.Disconnect();

				foreach (var client in gate.Clients)
					state.DisconnectingClients.Enqueue(client.Value);
			}
		}
	}
}
