using Mir.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.Network
{
    public interface IListener
    {
        IEnumerable<IConnection> Connections { get; }

        event EventHandler<IConnection> OnClientConnect;
        event EventHandler<IConnection> OnClientDisconnect;
        event EventHandler<Message> OnClientData;
        Task Listen(CancellationToken cancellationToken);
    }
}
