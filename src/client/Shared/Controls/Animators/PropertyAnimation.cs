using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls.Animators
{
    public class PropertyAnimation : Animation
    {
        private readonly Action<BaseControl, float> _propertySetter;


        public PropertyAnimation(Action<BaseControl, float> propertySetter, float from, float to, float steps, TimeSpan delay, bool loop = false)
            : base(from, to, steps, delay, loop)
        {
            _propertySetter = propertySetter;
        }

        protected override void Apply()
        {
            _propertySetter(Control, Current);
        }

    }
}
