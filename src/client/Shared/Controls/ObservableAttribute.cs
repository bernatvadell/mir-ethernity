using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ObservableAttribute : Attribute
    {
    }
}
