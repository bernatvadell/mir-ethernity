using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mir.Client.Models
{
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Keys[] keyData)
        {
            var otherKeys = new List<Keys>();

            for (var i = 0; i < keyData.Length; i++)
            {
                switch (keyData[i])
                {
                    case Keys.LeftAlt:
                    case Keys.RightAlt:
                        Alt = true;
                        break;
                    case Keys.LeftControl:
                    case Keys.RightControl:
                        Control = true;
                        break;
                    case Keys.LeftShift:
                    case Keys.RightShift:
                        Shift = true;
                        break;
                    default:
                        otherKeys.Add(keyData[i]);
                        break;
                }
            }

            PressedKeys = otherKeys.ToArray();
        }

        public virtual bool Alt { get; }
        public bool Control { get; }
        public virtual bool Shift { get; }
        public Keys[] PressedKeys { get; set; }
    }
}
