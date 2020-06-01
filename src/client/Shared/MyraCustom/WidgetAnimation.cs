using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
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

        public Action<MirWidget, int> Callback { get; private set; }
        public Action<MirWidget> EndCallback { get; private set; }
        public bool Loop { get; private set; }
        public MirWidget Self { get; private set; }
        public bool Enabled { get; private set; }
        public int CountIndex { get; private set; }
        public int FromIndex { get; private set; }
        public int ToIndex { get; private set; }
        public TimeSpan ElapseSpan { get; private set; }
        public TimeSpan ElapsePerFrame { get; private set; }
        public int CurrentIndex { get; private set; }


        private WidgetAnimation()
        {
            Enabled = true;
            Self = null;
            FromIndex = 0;
            ToIndex = 0;
            ElapseSpan = TimeSpan.FromMilliseconds(120);
            Callback = null;
            Loop = false;
        }

        public WidgetAnimation Attach(MirWidget widget)
        {
            Self = widget;
            Reset();
            return this;
        }

        public WidgetAnimation From(int from)
        {
            FromIndex = from;
            UpdateElapsePerFrame();
            return this;
        }

        public WidgetAnimation Count(int value)
        {
            CountIndex = value;
            ToIndex = FromIndex + value;
            UpdateElapsePerFrame();
            return this;
        }

        public WidgetAnimation To(int to)
        {
            CountIndex = ToIndex - FromIndex;
            ToIndex = to;
            UpdateElapsePerFrame();
            return this;
        }

        public WidgetAnimation Elapse(TimeSpan elapse)
        {
            ElapseSpan = elapse;
            UpdateElapsePerFrame();
            return this;
        }

        public WidgetAnimation WithCallback(Action<MirWidget, int> callback)
        {
            Callback = callback;
            return this;
        }

        public WidgetAnimation WithLoop()
        {
            Loop = true;
            return this;
        }

        public WidgetAnimation OnEnd(Action<MirWidget> endCallback)
        {
            EndCallback = endCallback;
            return this;
        }

        public WidgetAnimation WithoutLoop()
        {
            Loop = false;
            return this;
        }


        public void Update()
        {
            if (!Enabled) return;

            if (Time.TotalGameTime >= _nextTick)
            {
                CurrentIndex++;

                if (CurrentIndex <= ToIndex)
                {
                    Callback?.Invoke(Self, CurrentIndex);
                }
                else if (!Loop)
                {
                    Enabled = false;
                    EndCallback?.Invoke(Self);
                    return;
                }
                else
                {
                    Reset();
                    Callback?.Invoke(Self, CurrentIndex);
                }
                _nextTick = Time.TotalGameTime.Add(ElapsePerFrame);
            }
        }

        public void Reset()
        {
            _nextTick = Time.TotalGameTime.Add(ElapsePerFrame);
            CurrentIndex = FromIndex;
        }

        private void UpdateElapsePerFrame()
        {
            var a = ToIndex - FromIndex;
            if (a == 0) return;
            ElapsePerFrame = TimeSpan.FromTicks(ElapseSpan.Ticks / a);
        }

        public static WidgetAnimation Create()
        {
            return new WidgetAnimation();
        }
    }
}
