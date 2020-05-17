using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.GameServer.Models
{
    public class DatabaseOptions
    {
        public ProviderType Provider { get; set; }
        public string ConnectionString { get; set; }
    }
}
