using Mir.GameServer.Models;
using Mir.GameServer.Services.PacketProcessor;
using Mir.Network;
using Mir.Packets;
using Mir.Packets.Gate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.LoopTasks
{
    public class ProcessIncommingPackets : ILoopTask
    {
        private PacketProcessExecutor _packetProcessExecutor;

        public int Order => 1;

        public ProcessIncommingPackets(PacketProcessExecutor packetProcessExecutor)
        {
            _packetProcessExecutor = packetProcessExecutor;
        }

        public async Task Update(GameState state)
        {
            while (state.IncommingPackets.TryDequeue(out Message message))
            {
                if (!state.Gates.TryGetValue(message.Connection.Handle, out GateConnection gate))
                    continue;

                switch (message.Packet)
                {
                    case ClientPacket packet:
                        await OnClientPacket(gate, packet.SocketHandle, packet.Packet);
                        break;
                    case ClientConnectionChanged packet:
                        await OnClientConnectionChanged(state, gate, packet);
                        break;
                }
            }
        }

        private async Task OnClientPacket(GateConnection gate, int handle, Packet packet)
        {
            if (!gate.Clients.TryGetValue(handle, out ClientState client))
                return;

            await _packetProcessExecutor.Execute(client, packet);
        }

        private async Task OnClientConnectionChanged(GameState state, GateConnection gate, ClientConnectionChanged packet)
        {
            if (packet.Connected)
            {
                gate.Clients.TryAdd(packet.SocketHandle, new ClientState(gate, packet.SocketHandle));
            }
            else
            {
                if (gate.Clients.TryRemove(packet.SocketHandle, out ClientState client))
                    await client.Disconnect("Client disconnection");
            }
        }
    }
}
