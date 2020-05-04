using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services
{
    public interface IListener
    {
        bool Listening { get; }
        Task Listen(CancellationToken cancellationToken = default);
    }
}
