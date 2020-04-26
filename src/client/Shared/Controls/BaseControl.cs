using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mir.Client.Controls
{
    public abstract partial class BaseControl
    {
        public uint ScreenX { get; set; }
        public uint ScreenY { get; set; }

        public ControlCollection Controls { get; private set; }
        public BaseControl Parent { get; private set; }

        public BaseControl()
        {
            Controls = new ControlCollection(this);
        }
    }
}
