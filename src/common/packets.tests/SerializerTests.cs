using Mir.Packets;
using Mir.Packets.Common;
using Mir.Packets.Gate;
using System;
using Xunit;

namespace Packets.Tests
{
    public class SerializerTests
    {
        [Fact]
        public void SerializePingTest()
        {
            var packet = new Ping();
            var buffer = PacketSerializer.Serialize(packet);
            Assert.Equal(new byte[] { 35, 0, 0, 0, 0, 0, 0, 33 }, buffer);
        }

        [Fact]
        public void SerializePongTest()
        {
            var packet = new Pong();
            var buffer = PacketSerializer.Serialize(packet);
            Assert.Equal(new byte[] { 35, 0, 0, 0, 0, 1, 0, 33 }, buffer);
        }

        [Fact]
        public void SerializeClientPacketTest()
        {
            var packet = new ClientPacket() { SocketHandle = 100, Packet = new byte[] { 1, 2, 3, 4, 5 } };
            var buffer = PacketSerializer.Serialize(packet);
            Assert.Equal(new byte[] { 35, 9, 0, 0, 0, 2, 0, 8, 100, 18, 5, 1, 2, 3, 4, 5, 33 }, buffer);
        }
    }
}
