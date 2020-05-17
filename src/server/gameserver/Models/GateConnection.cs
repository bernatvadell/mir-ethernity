using Mir.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mir.GameServer.Models
{
    public class GateConnection
    {
        public IConnection Connection { get; }

        public ConcurrentDictionary<int, ClientState> Clients { get; } = new ConcurrentDictionary<int, ClientState>();

        public GateConnection(IConnection connection)
        {
            Connection = connection;
        }

        public async Task Disconnect()
        {
            foreach (var client in Clients)
                await client.Value.Disconnect("Game server shutdown");

            await Connection.Disconnect();
        }
    }
}
