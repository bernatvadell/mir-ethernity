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


        public bool CheckProcess(GameTime gameTime)
        {
            if (_nextTick == TimeSpan.Zero)
            {
                _nextTick = gameTime.TotalGameTime.Add(_elapse);
                return false;
            }

            if (gameTime.TotalGameTime >= _nextTick)
            {
                _nextTick = gameTime.TotalGameTime.Add(_elapse);
                return true;
            }

            return false;
        }
    }
}
