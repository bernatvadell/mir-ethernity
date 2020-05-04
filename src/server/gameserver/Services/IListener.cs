using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GameServer.Services
{
    public interface IListener
    {
        IReadOnlyCollection<IConnection> Connections { get; }
        bool Listening { get; }
        Task Listen(CancellationToken cancellationToken = default(CancellationToken));
    }
}
