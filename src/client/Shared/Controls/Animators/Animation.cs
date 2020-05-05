using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Controls.Animators
{
    public abstract class Animation
    {
        private bool _running = true;

        public BaseControl Control { get; private set; }

        public float From { get; set; }
        public float To { get; set; }
        public float Current { get; set; }
        public float Steps { get; set; }

        public TimeSpan DelayFrame { get; set; }
        public TimeSpan Delay { get; set; }
        public TimeSpan NextTick { get; set; }
        public bool Loop { get; set; }
        public bool Enabled { get; set; } = true;

        public event EventHandler Completed;

        public Animation(float from, float to, float steps, TimeSpan delay, bool loop = false)
        {
            From = from;
            To = to;
            Delay = delay;
            Loop = loop;
            Current = from;
            Steps = steps;
            DelayFrame = TimeSpan.FromTicks((long)((float)delay.Ticks / ((to - from) / steps)));
            NextTick = TimeSpan.Zero;
        }

        public void AttachToControl(BaseControl control)
        {
            Control = control;
        }

        public void Reset()
        {
            Current = From;
            DelayFrame = TimeSpan.FromMilliseconds(Delay.TotalMilliseconds / (double)((To - From)));
            _running = true;
        }

        public void Process(GameTime gameTime)
        {
            if (!_running || !Enabled || Control == null) return;

            if (gameTime.TotalGameTime >= NextTick)
            {
                Current += Steps;
                NextTick = gameTime.TotalGameTime + DelayFrame;

                if (Current >= To)
                {
                    if (!Loop)
                    {
                        _running = false;
                        Apply();
                        Completed?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    else
                    {
                        Current = From;
                        OnLoopCompleted();
                    }
                }

                Apply();
            }
        }

        protected virtual void OnLoopCompleted() { }

        protected abstract void Apply();
    }
}
