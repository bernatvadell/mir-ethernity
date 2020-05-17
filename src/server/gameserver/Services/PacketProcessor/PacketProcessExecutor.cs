using Autofac;
using Mir.GameServer.Models;
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

        public PacketProcessExecutor(ILifetimeScope scope)
        {
            _scope = scope;
            _baseType = typeof(PacketProcess<>);
        }

        public async Task Execute(ClientState client, Packet packet)
        {
            var packetType = packet.GetType();
            var processorType = _baseType.MakeGenericType(packetType);
            
            if (!_scope.IsRegistered(processorType))
                return;

            var processMethod = processorType.GetMethod("Process");

            var executor = _scope.Resolve(processorType);

            var task = (Task)processMethod.Invoke(executor, new object[] { client, packet });
            await task;
        }
    }
}
