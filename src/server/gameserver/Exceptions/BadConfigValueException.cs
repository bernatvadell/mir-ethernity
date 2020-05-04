using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.GameServer.Exceptions
{
    public class BadConfigValueException : Exception
    {
        public BadConfigValueException(string key, string value, string expected) : base($"A bad value for config {key}, value: {value}, format expected: {expected}") { }
    }
}
