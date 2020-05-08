using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Models
{
    public class TimeController
    {
        private TimeSpan _elapse;

        private TimeSpan _nextTick;

        public TimeController(TimeSpan elapse)
        {
            _elapse = elapse;
            _nextTick = TimeSpan.Zero;
        }

        public void Reset(TimeSpan elapse)
        {
            _elapse = elapse;
            _nextTick = TimeSpan.Zero;
        }

        public void Reset()
        {
            _nextTick = TimeSpan.Zero;
        }

        public bool CheckProcess()
        {
            if (_nextTick == TimeSpan.Zero)
            {
                _nextTick = Envir.Time.TotalGameTime.Add(_elapse);
                return false;
            }

            if (Envir.Time.TotalGameTime >= _nextTick)
            {
                _nextTick = Envir.Time.TotalGameTime.Add(_elapse);
                return true;
            }

            return false;
        }
    }
}
