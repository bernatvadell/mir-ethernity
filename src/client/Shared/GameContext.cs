using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Controls;
using Mir.Client.Exceptions;
using Mir.Client.Models;
using Mir.Client.Scenes;
using Mir.Client.Scenes.Splash;
using Mir.Client.Services;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Mir.Client
{
    public abstract class GameContext : Game
    {
        private readonly TimeController _fpsController = new TimeController(TimeSpan.FromSeconds(1));
        private readonly TimeController _upsController = new TimeController(TimeSpan.FromSeconds(1));

        private int _fpsCounter = 0;
        private int _upsCounter = 0;

        protected ILifetimeScope Container { get; private set; }
        protected ISceneManager SceneManager { get; private set; }
        protected IContentAccess ContentAccess { get; private set; }
        protected IDrawerManager DrawerManager { get; private set; }
        protected ICollection<IGamePadService> GamePadServices { get; private set; }

        public int FPS { get; private set; }
        public int UPS { get; private set; }

        public GameContext(ILifetimeScope container)
        {
            Content.RootDirectory = "Content";
            Container = container ?? throw new ArgumentNullException(nameof(container));
            IsFixedTimeStep = Config.FPSCap;
            MyraEnvironment.Game = this;
            Window.TextInput += Window_TextInput;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            Desktop.OnChar(e.Character);
        }

        protected override void LoadContent()
        {
            Desktop.HasExternalTextInput = true;

            ContentAccess = Container.Resolve<IContentAccess>();
            DrawerManager = Container.Resolve<IDrawerManager>();
            SceneManager = Container.Resolve<ISceneManager>();
            GamePadServices = Container.Resolve<ICollection<IGamePadService>>();

            ContentAccess.LoadContent();

            SceneManager.Load<SplashScene>(throwException: false);

            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_upsController.CheckProcess(gameTime))
            {
                UPS = _upsCounter;
                _upsCounter = 0;
            }
            try
            {
                UpdateScene(gameTime);
            }
            catch (SceneChangedException)
            {
                UpdateScene(gameTime);
            }

            base.Update(gameTime);
            _upsCounter++;
        }

        private void UpdateScene(GameTime gameTime)
        {
            foreach (var gps in GamePadServices)
                gps.Update(gameTime);
            SceneManager.Active.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (_fpsController.CheckProcess(gameTime))
            {
                FPS = _fpsCounter;
                _fpsCounter = 0;
            }

            DrawerManager.Clear(Color.Black);
            SceneManager.Active.Draw(gameTime);

            var debugLabel = $"FPS: {FPS} - UPS: {UPS}, Hover Control: { BaseControl.HoverControl?.Id ?? "None" }, Focus Control: { BaseControl.FocusControl?.Id ?? "None" }";

            using (var ctx = DrawerManager.PrepareSpriteBatch())
                ctx.Instance.DrawString(ContentAccess.Fonts[FontType.Normal], debugLabel, new Vector2(10, 10), Color.White);

            Desktop.Render();

            base.Draw(gameTime);

            _fpsCounter++;
        }
    }
}
