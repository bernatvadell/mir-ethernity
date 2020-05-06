using Autofac;
using Mir.Client.Controls;
using Mir.Client.Services;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Login.Views
{
    public class LoginBox : ImageControl
    {
        public LoginBox(ILifetimeScope scope) : base(scope)
        {
            Id = nameof(LoginBox);
            Library = LibraryType.Interface1c;
            Index = 2;
            Bottom = 100;
            HorizontalCenter = true;

            CreateControl<TextBoxControl>((control) =>
            {
                control.Id = "txt_id";
                control.Width = 184;
                control.Height = 16;
                control.Left = 68;
                control.Top = 64;
                control.Focus();
            });

            CreateControl<TextBoxControl>((control) =>
            {
                control.Id = "txt_password";
                control.Width = 184;
                control.Height = 16;
                control.Left = 355;
                control.Top = 64;
                control.Type = TextBoxInputType.Password;
            });

            CreateControl<ButtonControl>((control) =>
            {
                control.Id = "btn_login";
                control.Left = 550;
                control.Top = 60;
                control.Text = "Login";
            });

            CreateControl<ButtonControl>((control) =>
            {
                control.Id = "btn_exit";
                control.Left = 680;
                control.Top = 60;
                control.Text = "Exit";
            });
        }
    }
}
