using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mir.Packets
{
    public class PacketTypeCache
    {
        public Type Type { get; set; }
        public PacketAttribute Attribute { get; set; }
    }

    public class PacketSerializer
    {
        public static int MaxPacketSize = 1024 * 1024 * 4;
        public static IDictionary<PacketIndex, PacketTypeCache> PacketsType { get; }

        static PacketSerializer()
        {
            var packetBaseType = typeof(Packet);

            var packetsType = typeof(Packet).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOf(packetBaseType))
                .Select(x => new PacketTypeCache { Attribute = x.GetCustomAttribute<PacketAttribute>(false), Type = x })
                .ToList();

            foreach (var packet in packetsType)
            {
                if (packet.Attribute == null)
                    throw new ApplicationException($"Packet: {packet.Type.FullName}, not have decorator [Packet(...)]");
            }

            PacketsType = packetsType.ToDictionary(x => x.Attribute.Index, x => x);
        }


        public static Packet Deserialize(byte[] bufferIn, out byte[] bufferExtra, PacketSource source)
        {
            // we need more data for process it...
            if (bufferIn.Length < 8)
            {
                bufferExtra = bufferIn;
                return null;
            }

            if (bufferIn[0] != '#')
                throw new WrongPacketException();

            var length = BitConverter.ToUInt32(bufferIn, 1);

            if (length > MaxPacketSize)
                throw new WrongPacketException();

            // we need more data for process it...
            if (bufferIn.Length < length + 8)
            {
                bufferExtra = bufferIn;
                return null;
            }

            if (bufferIn[7 + length] != '!')
                throw new WrongPacketException();

            var packetIndex = (PacketIndex)BitConverter.ToUInt16(bufferIn, 5);
            if (!PacketsType.ContainsKey(packetIndex))
                throw new WrongPacketException();

            var packetType = PacketsType[packetIndex];

            if (packetType.Attribute.Source != PacketSource.Common && packetType.Attribute.Source != source)
                throw new WrongPacketException();

            var buffer = new byte[length];
            Array.Copy(bufferIn, 7, buffer, 0, length);

            bufferExtra = new byte[bufferIn.Length - 8 - length];

            if (bufferExtra.Length > 0)
                Array.Copy(bufferIn, 8 + length, bufferExtra, 0, bufferExtra.Length);

            using (var ms = new MemoryStream(buffer))
                return (Packet)Serializer.Deserialize(packetType.Type, ms);
        }

        public static byte[] Serialize(Packet packet)
        {
            var packetType = packet.GetType();
            var attribute = packetType.GetCustomAttribute<PacketAttribute>(false);
            if (attribute == null) throw new WrongPacketException();

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, packet);
                var buffer = ms.ToArray();
                var output = new byte[buffer.Length + 8];
                output[0] = (byte)'#';
                output[output.Length - 1] = (byte)'!';
                Array.Copy(BitConverter.GetBytes(buffer.Length), 0, output, 1, 4);
                Array.Copy(BitConverter.GetBytes((ushort)attribute.Index), 0, output, 5, 2);
                Array.Copy(buffer, 0, output, 7, buffer.Length);

                return output;
            }
        }
    }
}
