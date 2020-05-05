using Autofac;
using Mir.Client.Controls;
using Mir.Client.Controls.Animators;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Login.Views
{
    public class IntroBackground : ImageControl
    {
        public IntroBackground(ILifetimeScope scope) : base(scope)
        {
            Id = "Background";
            Library = LibraryType.Interface1c;
            Index = 20;

            CreateControl<ImageControl>((control) => 
            {
                control.Id = "Birds";
                control.AddAnimator(new PropertyAnimation((c, v) => control.Index = (int)v, 2200, 2289, 1, TimeSpan.FromSeconds(10), loop: true));
                control.Library = LibraryType.Interface1c;
                control.UseOffset = true;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "Flags";
                control.AddAnimator(new PropertyAnimation((c, v) => control.Index = (int)v, 2400, 2430, 1, TimeSpan.FromSeconds(3), loop: true));
                control.Library = LibraryType.Interface1c;
                control.UseOffset = true;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "SunLight";
                control.AddAnimator(new PropertyAnimation((c, v) => control.Index = (int)v, 2300, 2330, 1, TimeSpan.FromSeconds(5), loop: true));
                control.Library = LibraryType.Interface1c;
                control.UseOffset = true;
                control.UseBlend = true;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "Water";
                control.AddAnimator(new PropertyAnimation((c, v) => control.Index = (int)v, 2500, 2530, 1, TimeSpan.FromSeconds(3), loop: true));
                control.Library = LibraryType.Interface1c;
                control.UseOffset = true;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "LogoShadow";
                control.Index = 23;
                control.HorizontalCenter = true;
                control.Top = 100;
                control.Library = LibraryType.Interface1c;
                control.Opacity = 0.9f;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "Logo";
                control.Index = 22;
                control.HorizontalCenter = true;
                control.Top = 66;
                control.UseBlend = true;
                control.Library = LibraryType.Interface1c;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "PegiA";
                control.Index = 3;
                control.Right = 0;
                control.Library = LibraryType.Interface1c;
            });

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "PegiB";
                control.Index = 4;
                control.Right = 0;
                control.Top = 100;
                control.Library = LibraryType.Interface1c;
            });
        }
    }
}
