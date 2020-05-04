using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mir.GateServer.Services.Default
{
    public class GateService : IService
    {
        private readonly IListener _listener;

        public GateService(IListener listener)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            await _listener.Listen(cancellationToken);
        }
    }
}
