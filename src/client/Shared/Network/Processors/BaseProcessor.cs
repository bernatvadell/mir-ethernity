using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Network.Processors
{
    public abstract class BaseProcessor<TPacket>
    {
        public abstract void Process(TPacket packet);
    }
}
