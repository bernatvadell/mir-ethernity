using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.Network
{
    public interface IClient
    {
        bool Connected { get; }

        event EventHandler OnDisconnect;
        event EventHandler<Packet> OnData;

        Task Connect(CancellationToken cancellationToken = default);
        Task Send(Packet packet);
        void Disconnect();
    }
}
