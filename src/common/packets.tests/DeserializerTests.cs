using Mir.Packets;
using Mir.Packets.Common;
using Mir.Packets.Gate;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Packets.Tests
{
    public class DeserializerTests
    {
        [Fact]
        public void DeserializePingTest()
        {
            var buffer = new byte[] { 35, 0, 0, 0, 0, 0, 0, 33 };
            var packet = PacketSerializer.Deserialize(buffer, out byte[] bufferExtra, PacketSource.Client);
            Assert.IsType<Ping>(packet);
            Assert.Empty(bufferExtra);
        }

        [Fact]
        public void DeserializePongTest()
        {
            var buffer = new byte[] { 35, 0, 0, 0, 0, 1, 0, 33 };
            var packet = PacketSerializer.Deserialize(buffer, out byte[] bufferExtra, PacketSource.Client);
            Assert.IsType<Pong>(packet);
            Assert.Empty(bufferExtra);
        }

        [Fact]
        public void DeserializeClientPacketTest()
        {
            var buffer = new byte[] { 35, 9, 0, 0, 0, 2, 0, 8, 100, 18, 5, 1, 2, 3, 4, 5, 33 };
            var packet = PacketSerializer.Deserialize(buffer, out byte[] bufferExtra, PacketSource.Gate);
            Assert.IsType<ClientPacket>(packet);
            var npacket = (ClientPacket)packet;
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, npacket.Packet);
            Assert.Equal(100, npacket.SocketHandle);
            Assert.Empty(bufferExtra);
        }
    }
}
