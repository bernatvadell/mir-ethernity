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
            var animation = WidgetAnimation.Create()
                .WithCallback(callback)
                .From(from)
                .To(to)
                .Elapse(elapse)
                .Attach(this);

            if (loop)
                animation.WithLoop();
            else
                animation.WithoutLoop();

            _animations.Add(animation);

            return this;
        }

        public MirWidget WithAnimation(WidgetAnimation animation)
        {
            _animations.Add(animation);
            animation.Attach(this);
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
            {
                _animations[i].Update();
                if (!_animations[i].Enabled)
                {
                    _animations.RemoveAt(i);
                    i--;
                }
            }

           // base.InternalRender(context);
        }
    }
}
