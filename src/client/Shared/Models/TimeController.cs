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
        private bool _runOnFirstCheck;

        public TimeController(TimeSpan elapse, bool runOnFirstCheck = false)
        {
            _elapse = elapse;
            _nextTick = TimeSpan.Zero;
            _runOnFirstCheck = runOnFirstCheck;
        }

        public void Reset(TimeSpan elapse)
        {
            _elapse = elapse;
            _nextTick = Envir.Time.TotalGameTime.Add(_elapse);
        }

        public void Reset()
        {
            _nextTick = Envir.Time.TotalGameTime.Add(_elapse);
        }

        public bool CheckProcess()
        {
            if (_nextTick == TimeSpan.Zero)
            {
                _nextTick = Envir.Time.TotalGameTime.Add(_elapse);
                return _runOnFirstCheck;
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
