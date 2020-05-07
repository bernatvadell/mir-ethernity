using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using Mir.Client.Services;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir.Client
{
    public class WindowsGameContext : GameContext
    {
      
        public WindowsGameContext(ILifetimeScope container) : base(container)
        {
            IsMouseVisible = true;
            Window.TextInput += Window_TextInput;

        }

        private void Window_TextInput(object sender, Microsoft.Xna.Framework.TextInputEventArgs e)
        {
            BaseControl.FocusControl?.OnTextInput(e.Key, e.Character);
        }
    }
}
