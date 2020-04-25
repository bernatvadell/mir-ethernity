using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Exceptions
{
    public class ServiceNotSpecifiedException : Exception
    {
        public ServiceNotSpecifiedException(string name) : base($"Service {name} not specified")
        {

        }
    }
}
