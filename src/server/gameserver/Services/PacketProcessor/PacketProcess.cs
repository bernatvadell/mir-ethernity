using Mir.GameServer.Models;
using Mir.Network;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mir.GameServer.Services.PacketProcessor
{
    public abstract class PacketProcess<TPacket> where TPacket : Packet
    {
        public abstract Stage Stage { get; }

        public async Task Process(ClientState client, TPacket packet)
        {
            if (client.Stage != Stage)
            {
                await client.Disconnect("Receiving invalid stage packets");
                return;
            }
            await ProcessPacket(client, packet);
        }

        protected abstract Task ProcessPacket(ClientState client, TPacket packet);

    }
}
