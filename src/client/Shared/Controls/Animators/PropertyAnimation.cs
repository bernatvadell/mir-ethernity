using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls.Animators
{
    public class PropertyAnimation : Animation
    {
        private readonly Action<BaseControl, float> _propertySetter;

        public bool Reverse { get; set; }

        public PropertyAnimation(Action<BaseControl, float> propertySetter, float from, float to, float steps, TimeSpan delay, bool reverse = false, bool loop = false)
            : base(from, to, steps, delay, loop)
        {
            _propertySetter = propertySetter;
            Reverse = reverse;
        }

        protected override void Apply()
        {
            _propertySetter(Control, Reverse ? To - Current : Current);
        }

        protected override void OnLoopCompleted()
        {
            Reverse = !Reverse;
        }
    }
}
