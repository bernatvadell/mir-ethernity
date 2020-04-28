using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Exceptions;
using Mir.Client.Models;
using Mir.Client.Scenes;
using Mir.Client.Scenes.Splash;
using Mir.Client.Services;
using System;

namespace Mir.Client
{
    public class GameWindow : Game
    {
        private readonly ILifetimeScope _container;
        private readonly ISceneManager _sceneManager;
        private IContentAccess _contentAccess;
        private IDrawerManager _drawerManager;

        private readonly TimeController _fpsController = new TimeController(TimeSpan.FromSeconds(1));
        private readonly TimeController _upsController = new TimeController(TimeSpan.FromSeconds(1));

        private int _fpsCounter = 0;
        private int _upsCounter = 0;

        public int FPS { get; private set; }
        public int UPS { get; private set; }

        public GameWindow(ISceneManager sceneManager, ILifetimeScope container)
        {
            Content.RootDirectory = "Content";
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));

            IsFixedTimeStep = Config.FPSCap;
        }

        protected override void LoadContent()
        {
            _contentAccess = _container.Resolve<IContentAccess>();
            _drawerManager = _container.Resolve<IDrawerManager>();

            _contentAccess.LoadContent();

            _sceneManager.Load<SplashScene>(throwException: false);

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
                _sceneManager.Active.Update(gameTime);
            }
            catch (SceneChangedException)
            {
                _sceneManager.Active.Update(gameTime);
            }

            base.Update(gameTime);
            _upsCounter++;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (_fpsController.CheckProcess(gameTime))
            {
                FPS = _fpsCounter;
                _fpsCounter = 0;
            }

            _sceneManager.Active.Draw(gameTime);

            using (var ctx = _drawerManager.PrepareSpriteBatch())
                ctx.Data.DrawString(_contentAccess.Fonts[FontType.Normal], $"FPS: {FPS} - UPS: {UPS}", new Vector2(10, 10), Color.White);

            base.Draw(gameTime);

            _fpsCounter++;
        }
    }
}
