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
            public int Length { get => _controls.Count; }

            public BaseControl this[int index]
            {
                get
                {
                    return _controls.Count > index ? _controls[index] : null;
                }
            }

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

            internal void Dispose()
            {
                for (var i = 0; i < _controls.Count; i++)
                    _controls[i].Dispose();
                _controls.Clear();
                _controls = null;
            }
        }
    }
}
