using Mir.Packets;
using Mir.Packets.Client;
using Mir.Packets.Common;
using Mir.Packets.Gate;
using System;
using System.IO;
using Xunit;

namespace Packets.Tests
{
	public class SerializerTests
	{
		[Fact]
		public void SerializePingTest()
		{
			var packet = new Ping();
			var buffer = Serialize(packet);
			Assert.Equal(new byte[] { 10, 0 }, buffer);
		}

		[Fact]
		public void SerializePongTest()
		{
			var packet = new Pong();
			var buffer = Serialize(packet);
			Assert.Equal(new byte[] { 18, 0 }, buffer);
		}

		[Fact]
		public void SerializeClientPacketTest()
		{
			var packet = new ClientPacket() { SocketHandle = 100, Packet = new Login { Username = "Test", Password = "Pass" } };
			var buffer = Serialize(packet);
			Assert.Equal(new byte[] { 202, 62, 19, 8, 100, 18, 15, 138, 125, 12, 10, 4, 84, 101, 115, 116, 18, 4, 80, 97, 115, 115 }, buffer);
		}

		private byte[] Serialize(Packet packet)
		{
			using (var ms = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(ms, packet);
				return ms.ToArray();
			}
		}
	}
}
