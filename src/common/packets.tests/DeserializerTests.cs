using Mir.Packets;
using Mir.Packets.Client;
using Mir.Packets.Common;
using Mir.Packets.Gate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Packets.Tests
{
	public class DeserializerTests
	{
		[Fact]
		public void DeserializePingTest()
		{
			var buffer = new byte[] { 10, 0 };
			var packet = Deserialize(buffer);

			Assert.IsType<Ping>(packet);
		}

		[Fact]
		public void DeserializePongTest()
		{
			var buffer = new byte[] { 18, 0 };
			var packet = Deserialize(buffer);
			Assert.IsType<Pong>(packet);
		}

		[Fact]
		public void DeserializeClientPacketTest()
		{
			var buffer = new byte[] { 202, 62, 19, 8, 100, 18, 15, 138, 125, 12, 10, 4, 84, 101, 115, 116, 18, 4, 80, 97, 115, 115 };
			var packet = Deserialize(buffer);
			Assert.IsType<ClientPacket>(packet);
			var npacket = (ClientPacket)packet;
			Assert.Equal(100, npacket.SocketHandle);
			Assert.IsType<Login>(npacket.Packet);
			var spacket = (Login)npacket.Packet;
			Assert.Equal("Test", spacket.Username);
			Assert.Equal("Pass", spacket.Password);
		}

		[Fact]
		public void SimulatePendingData()
		{
			var buffer = new byte[] { 202, 62, 19, 8, 100, 18, 15, 138, 125, 12, 10, 4, 84, 101, 115, 116, 18, 4 };
			var exception = Record.Exception(() => Deserialize(buffer));
			Assert.NotNull(exception);
			Assert.IsType<EndOfStreamException>(exception);
		}

		private Packet Deserialize(byte[] buffer)
		{
			using (var ms = new MemoryStream(buffer))
				return ProtoBuf.Serializer.Deserialize<Packet>(ms);
		}
	}
}
