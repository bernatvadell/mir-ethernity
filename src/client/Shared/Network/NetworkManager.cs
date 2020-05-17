using Mir.Client.MyraCustom;
using Mir.Network.TCP;
using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mir.Client.Network
{
    public class NetworkManager
    {
        public TCPNetworkClient Client { get; set; }

        public void OnLostConnection(object sender, EventArgs e)
        {
            MirWindow.ShowDialog("You are disconnected", "Connection lost, you want to go login?");
        }

        public void OnReceivePacket(object sender, Packet e)
        {

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
