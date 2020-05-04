using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services
{
    public interface IService
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}
