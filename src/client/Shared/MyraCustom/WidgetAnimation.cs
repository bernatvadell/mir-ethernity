using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Mir.Client.MyraCustom
{
    public class WidgetAnimation
    {
        public static GameTime Time { get; set; } = new GameTime();

        private TimeSpan _nextTick;

        public Action<MirWidget, int> Callback { get; }
        public bool Loop { get; }
        public MirWidget Self { get; }
        public bool Enabled { get; set; }
        public int From { get; }
        public int To { get; }
        public TimeSpan Elapse { get; }
        public TimeSpan ElapsePerFrame { get; }
        public int Current { get; private set; }

        public WidgetAnimation(Action<MirWidget, int> callback, MirWidget self, int from, int to, TimeSpan elapse, bool loop)
        {
            Enabled = true;

            Self = self;
            From = from;
            To = to;
            Elapse = elapse;
            ElapsePerFrame = TimeSpan.FromTicks(elapse.Ticks / (to - from));
            Callback = callback;
            Loop = loop;

            Reset();
        }

        public void Update()
        {
            if (!Enabled) return;

            if (Time.TotalGameTime >= _nextTick)
            {
                Current++;

                if (Current <= To)
                {
                    Callback(Self, Current);
                }
                else if (!Loop)
                {
                    Enabled = false;
                    return;
                }
                else
                {
                    Reset();
                    Callback(Self, Current);
                }
                _nextTick = Time.TotalGameTime.Add(ElapsePerFrame);
            }
        }

        public void Reset()
        {
            _nextTick = Time.TotalGameTime.Add(ElapsePerFrame);
            Current = From;
        }
    }
}
