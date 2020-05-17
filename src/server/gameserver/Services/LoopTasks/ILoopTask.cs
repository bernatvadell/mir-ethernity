using Mir.GameServer.Models;
using Mir.GameServer.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
	public interface ILoopTask
	{
		int Order { get; }
		Task Update(GameState state);
	}
}
