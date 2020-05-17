using Mir.Client.MyraCustom;
using Mir.Client.Network.Processors;
using Mir.Network.TCP;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using XNAssets.Utility;

namespace Mir.Client.Network
{
    public class NetworkManager
    {
        private Dictionary<Type, Type> _types;
        private Dictionary<Type, object> _packetSingleInstances = new Dictionary<Type, object>();

        public TCPNetworkClient Client { get; set; }

        public NetworkManager()
        {
            _types = typeof(BaseProcessor<>).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOfRawGeneric(typeof(BaseProcessor<>)))
                .ToDictionary(x => x.BaseType.GetGenericArguments()[0], x => x);
        }

        public void OnLostConnection(object sender, EventArgs e)
        {
            MirWindow.ShowDialog("You are disconnected", "Connection lost, you want to go login?");
        }

        public void OnReceivePacket(object sender, Packet e)
        {
            if (_types.TryGetValue(e.GetType(), out Type processorPacketType))
            {
                if (!_packetSingleInstances.TryGetValue(processorPacketType, out object instance))
                {
                    instance = Activator.CreateInstance(processorPacketType);
                    _packetSingleInstances.Add(processorPacketType, instance);
                }

                var process = processorPacketType.GetMethod("Process");
                process.Invoke(instance, new object[] { e });
            }
        }

        public void PrepareConnection()
        {
            Client?.Disconnect();

            Client = new TCPNetworkClient(new TCPNetworkClientOptions
            {
                ServerIP = IPAddress.Parse(Config.ServerIP),
                ServerPort = Config.Port,
                Source = PacketSource.Server
            });

            Client.OnDisconnect += OnLostConnection;
            Client.OnData += OnReceivePacket;
        }
    }
}
