using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public class TouchEventArgs : EventArgs
    {
        public BaseControl Target { get; set; }
        public Point Point { get; set; }
    }
}
