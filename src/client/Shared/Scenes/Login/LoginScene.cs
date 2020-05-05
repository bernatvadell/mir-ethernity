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
            Id = nameof(LoginScene);

            CreateControl<LabelControl>((c1) =>
            {
                c1.Text = "Esto es un texto de prueba";
                c1.Left = 100;
                c1.Top = 100;
                c1.Width = 300;
                c1.Height = 300;
                c1.BackgroundColor = Color.Red;
                c1.Color = Color.White;

                CreateControl<LabelControl>((c2) =>
                {
                    c2.Text = "Esto es un texto de prueba";
                    c2.Left = 100;
                    c2.Top = 100;
                    c2.Width = 200;
                    c2.Height = 200;
                    c2.BackgroundColor = Color.Blue;
                    c2.Color = Color.White;
                }, c1);
            });

            var loginBox = CreateControl<ImageControl>((control) =>
            {
                control.Id = "Image 1";
                control.Left = 10;
                control.Bottom = 10;
                control.LibraryType = Mir.Models.LibraryType.Interface1c;
                control.Index = 2;
            });

            CreateControl<TextBoxControl>((control) =>
            {
                control.Id = "Textbox";
                control.Width = 184;
                control.Height = 16;
                control.Left = 68;
                control.Top = 64;

                control.Focus();
            }, loginBox);
        }
    }
}
