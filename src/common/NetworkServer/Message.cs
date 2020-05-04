using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Network
{
    public class Message : EventArgs
    {
        public IConnection Connection { get; }
        public Packet Packet { get; }

        public Message(IConnection connection, Packet packet)
        {
            Connection = connection;
            Packet = packet;
        }
    }
}
