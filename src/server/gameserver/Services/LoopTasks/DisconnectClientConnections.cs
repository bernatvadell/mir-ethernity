using Mir.GameServer.Models;
using Mir.GameServer.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
	public class DisconnectClientConnections : ILoopTask
	{
		public int Order => 1;

		public Task Update(GameState state)
		{
			while (state.DisconnectingClients.TryDequeue(out ClientState client))
			{
				if (client.GateConnection.Connection.Connected)
				{
					// state.GateMessages
				}
				client.GateConnection.Clients.Remove(client.SocketHandle);
			}

			return Task.CompletedTask;
		}
	}
}
