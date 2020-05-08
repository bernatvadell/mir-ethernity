using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Models;
using Mir.Client.MyraCustom;
using Mir.Client.Scenes.Login;
using Mir.Client.Services;
using Mir.Models;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Splash
{
    public class SplashScene : BaseScene
    {
        private TimeController _splashTimeController = new TimeController(TimeSpan.FromSeconds(3));

        public SplashScene()
        {
            Widgets.Add(new MirImage
            {
                Index = 6,
                Library = LibraryType.Interface1c
            }.WithAnimation((s, e) => s.Opacity = (100 - e) / 100f, 0, 100, TimeSpan.FromSeconds(2), false));


        }

        public override void Update()
        {
            if (_splashTimeController.CheckProcess())
                SceneManager.Instance.Load(new LoginScene());

            base.Update();
        }
    }
}
