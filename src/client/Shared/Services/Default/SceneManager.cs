using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Exceptions;
using Mir.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class SceneManager : ISceneManager
    {
        private readonly ILifetimeScope _container;

        public BaseScene Active { get; private set; }

        public SceneManager(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Load<TScene>(bool throwException = true) where TScene : BaseScene
        {
            Active?.Dispose();

            var scene = _container.Resolve<TScene>();
            var device = _container.Resolve<GraphicsDeviceManager>();

            scene.Width = device.PreferredBackBufferWidth;
            scene.Height = device.PreferredBackBufferHeight;
            scene.BackgroundColor = Color.Black;

            Active = scene;

            if (throwException) throw new SceneChangedException();
        }
    }
}
