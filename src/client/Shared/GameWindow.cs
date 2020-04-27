using Autofac;
using Microsoft.Xna.Framework;
using Mir.Client.Scenes;
using Mir.Client.Services;
using System;

namespace Mir.Client
{
    public class GameWindow : Game
    {
        private readonly ISceneManager _sceneManager;
        private readonly ILifetimeScope _container;
        private readonly GraphicsDeviceManager _graphics;

        public GameWindow(ISceneManager sceneManager, ILifetimeScope container)
        {
            Content.RootDirectory = "Content";
            _sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _graphics = _container.Resolve<GraphicsDeviceManager>(new TypedParameter(typeof(Game), this));
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Initialize()
        {
            _sceneManager.Load<GameScene>();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _sceneManager.Active.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _sceneManager.Active.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
