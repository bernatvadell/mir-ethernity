using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Exceptions;
using Mir.Client.Models;
using Mir.Client.MyraCustom;
using Mir.Client.Scenes;
using Mir.Client.Scenes.Splash;
using Mir.Client.Services;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Mir.Client
{
    public class GameContext : Game
    {
        private readonly TimeController _fpsController = new TimeController(TimeSpan.FromSeconds(1));
        private readonly TimeController _upsController = new TimeController(TimeSpan.FromSeconds(1));

        private int _fpsCounter = 0;
        private int _upsCounter = 0;

        protected ILifetimeScope Container { get; private set; }
        protected IContentAccess ContentAccess { get; private set; }
        protected IDrawerManager DrawerManager { get; private set; }

        public int FPS { get; private set; }
        public int UPS { get; private set; }

        public GameContext(ILifetimeScope container)
        {
            Envir.Game = this;

            Content.RootDirectory = "Content";
            Container = container ?? throw new ArgumentNullException(nameof(container));
            IsFixedTimeStep = Config.FPSCap;
            MyraEnvironment.Game = this;

#if WINDOWS || LINUX
            IsMouseVisible = true;
            Window.TextInput += Window_TextInput;
#endif
        }

#if WINDOWS || LINUX
        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            Desktop.OnChar(e.Character);
        }
#endif

        protected override void LoadContent()
        {
            Desktop.HasExternalTextInput = true;

            ContentAccess = Container.Resolve<IContentAccess>();
            DrawerManager = Container.Resolve<IDrawerManager>();

            ContentAccess.LoadContent();

            Fonts.Instance.Load(Content);
            SceneManager.Instance.Load(new SplashScene(), throwException: false);

            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Envir.Time = gameTime;

            if (_upsController.CheckProcess())
            {
                UPS = _upsCounter;
                _upsCounter = 0;
            }

            if (_fpsController.CheckProcess())
            {
                FPS = _fpsCounter;
                _fpsCounter = 0;
            }

            try
            {
                SceneManager.Instance.Active?.Update();
            }
            catch (SceneChangedException)
            {
                SceneManager.Instance.Active?.Update();
            }

            base.Update(gameTime);
            _upsCounter++;
        }

        protected override void Draw(GameTime gameTime)
        {
            WidgetAnimation.Time = gameTime;

            DrawerManager.Clear(Color.Black);

            Desktop.Render();

            var debugLabel = $"FPS: {FPS} - UPS: {UPS}";

            using (var ctx = DrawerManager.PrepareSpriteBatch())
                ctx.Instance.DrawString(ContentAccess.Fonts[FontType.Normal], debugLabel, new Vector2(10, 10), Color.White);

            base.Draw(gameTime);

            _fpsCounter++;
        }
    }
}
