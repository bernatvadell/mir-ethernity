using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using Mir.Client.Controls.Animators;
using Mir.Client.Models;
using Mir.Client.Scenes.Login;
using Mir.Client.Services;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Splash
{
    public class SplashScene : BaseScene
    {
        private TimeController _splashTimeController = new TimeController(TimeSpan.FromSeconds(3));

        public SplashScene(ILifetimeScope container, IContentAccess contentAccess) : base(container)
        {
            Id = nameof(SplashScene);

            CreateControl<ImageControl>((control) =>
            {
                control.Id = "BackgroundImage";
                control.Library = LibraryType.Interface1c;
                control.Index = 6;
                control.Width = DrawerManager.Width;
                control.Height = DrawerManager.Height;
            });

            CreateControl<TextureControl>((control) =>
            {
                control.Id = "OpacityLayer";
                control.Width = DrawerManager.Width;
                control.Height = DrawerManager.Height;
                control.Opacity = 0;
                control.Texture = contentAccess.BlackBackground;
                control.AddAnimator(new PropertyAnimation((c, v) => c.Opacity = v, 0, 1, 0.01f, TimeSpan.FromSeconds(2)));
            });
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (_splashTimeController.CheckProcess(gameTime))
            {
                SceneManager.Load<LoginScene>();
            }
        }
    }
}
