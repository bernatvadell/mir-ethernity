using Autofac;
using Mir.Network;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.PacketProcessor
{
	public class PacketProcessExecutor
	{
		private readonly ILifetimeScope _scope;
		private readonly Type _baseType;
		private readonly MethodInfo _processMethod;

		public PacketProcessExecutor(ILifetimeScope scope)
		{
			_scope = scope;
			_baseType = typeof(PacketProcess<>);
			_processMethod = _baseType.GetMethod(nameof(PacketProcess<Packet>.Process));
		}

		public async Task Execute(IConnection connection, Packet packet)
		{
			var packetType = packet.GetType();
			var executor = _scope.Resolve(_baseType.MakeGenericType(packetType));
			var method = _processMethod.MakeGenericMethod(packetType);
			var task = (Task)method.Invoke(executor, new object[] { connection, packet });
			await task;
		}
	}
}
