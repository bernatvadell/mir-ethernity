using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls
{
    public abstract partial class BaseControl
    {
        public class ControlCollection
        {
            private BaseControl _parent;
            private List<BaseControl> _controls;

            public ControlCollection(BaseControl parent)
            {
                _parent = parent ?? throw new ArgumentNullException(nameof(parent));
                _controls = new List<BaseControl>();
            }

            public void Remove(BaseControl control)
            {
                _controls.Remove(control);
                control.Parent = null;
            }

            public void Add(BaseControl control)
            {
                control.Parent?.Controls.Remove(control);
                _controls.Add(control);
                control.Parent = _parent;
            }
        }
    }
}
