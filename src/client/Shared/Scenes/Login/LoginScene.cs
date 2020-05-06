using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using Mir.Client.Scenes.Login.Views;
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

             CreateControl<IntroBackground>();

            _loginBox = CreateControl<LoginBox>();
        }
    }
}
