using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using Mir.Client.Scenes.Login.Views;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Login
{
    public class LoginScene : BaseScene
    {
        private readonly LoginBox _loginBox;

        public LoginScene(ILifetimeScope container) : base(container)
        {
            Id = nameof(LoginScene);

            // CreateControl<IntroBackground>();

            //_loginBox = CreateControl<LoginBox>();

            var panel = new Panel();

            panel.Widgets.Add(new TextBox
            {
                Left = 100,
                Top = 100
            });
            
            Desktop.Root = panel;
        }
    }
}
