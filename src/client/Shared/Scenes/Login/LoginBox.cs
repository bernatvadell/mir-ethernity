using Microsoft.Xna.Framework;
using Mir.Client.MyraCustom;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Login
{
    public class LoginBox : Panel
    {
        private readonly MirButton _exitButton;
        private TextBox _idField;
        private TextBox _passField;
        private MirButton _loginButton;

        public LoginBox()
        {
            VerticalAlignment = VerticalAlignment.Bottom;
            HorizontalAlignment = HorizontalAlignment.Center;
            Top = -100;

            var background = new MirImageBrush()
            {
                Index = 2,
                Library = Mir.Models.LibraryType.Interface1c
            };
            Background = background;
            Width = background.Size.X;
            Height = background.Size.Y;

            Widgets.Add(_idField = new TextBox
            {
                Id = "txt_id",
                Left = 68,
                Top = 64,
                Width = 184,
                Height = 16,
                Background = new SolidBrush(Color.Transparent),
                Font = Fonts.Instance.Size8
            });
            _idField.KeyDown += IdField_KeyDown;
            _idField.SetKeyboardFocus();

            Widgets.Add(_passField = new TextBox
            {
                Id = "txt_pass",
                Left = 355,
                Top = 64,
                Width = 184,
                Height = 16,
                PasswordField = true,
                Background = new SolidBrush(Color.Transparent),
                Font = Fonts.Instance.Size8
            });

            Widgets.Add(_loginButton = new MirButton()
            {
                Text = "Login",
                Left = 550,
                Top = 60,
            });

            Widgets.Add(_exitButton = new MirButton()
            {
                Text = "Close",
                Left = 680,
                Top = 60,
            });
            _exitButton.Click += ExitButton_Click;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Envir.Game.Exit();
        }

        private void IdField_KeyDown(object sender, Myra.Utility.GenericEventArgs<Microsoft.Xna.Framework.Input.Keys> e)
        {
            if (e.Data == Microsoft.Xna.Framework.Input.Keys.Tab)
                _passField.SetKeyboardFocus();
        }
    }
}
