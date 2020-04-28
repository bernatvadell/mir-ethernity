using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Login
{
    public class LoginScene : BaseScene
    {
        public LoginScene(ILifetimeScope container) : base(container)
        {
            CreateControl<ImageControl>((control) =>
            {
                control.Left = 10;
                control.Bottom = 10;
                control.LibraryType = Mir.Models.LibraryType.Interface1c;
                control.Index = 2;
            });
        }
    }
}
