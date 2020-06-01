using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.MyraCustom
{


    public class MirWidget : Widget
    {
        private readonly List<WidgetAnimation> _animations = new List<WidgetAnimation>();

        public MirWidget WithAnimation(Action<MirWidget, int> callback, int from, int to, TimeSpan elapse, bool loop = false)
        {
            _animations.Add(new WidgetAnimation(callback, this, from, to, elapse, loop));
            callback(this, from); // force first call for set initial value
            return this;
        }

        public MirWidget ClearAnimations()
        {
            _animations.Clear();
            return this;
        }

        public override void InternalRender(RenderContext context)
        {
            for (var i = 0; i < _animations.Count; i++)
                _animations[i].Update();

            base.InternalRender(context);
        }
    }
}
