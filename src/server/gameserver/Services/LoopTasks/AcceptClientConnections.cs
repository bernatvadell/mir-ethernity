using Mir.GameServer.Models;
using Mir.GameServer.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
	public class AcceptClientConnections : ILoopTask
	{
		public int Order => 1;

		public Task Update(GameState state)
		{
			while (state.ConnectingClients.TryDequeue(out ClientState client))
			{
				if (client.GateConnection.Connection.Connected)
				{
					client.GateConnection.Clients.Add(client.SocketHandle, client);
				}
			}

			return Task.CompletedTask;
		}
	}
}
