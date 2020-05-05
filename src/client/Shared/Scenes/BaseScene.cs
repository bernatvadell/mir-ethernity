using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls;
using Mir.Client.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes
{
    public abstract class BaseScene : BaseControl
    {
        private readonly ILifetimeScope _container;
        private readonly GraphicsDevice _graphics;
        private readonly SpriteBatch _spriteBatch;

        protected ISceneManager SceneManager { get; private set; }

        public BaseScene(ILifetimeScope container) : base(container.Resolve<IDrawerManager>(), container.Resolve<IRenderTargetManager>())
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
            _graphics = container.Resolve<GraphicsDevice>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            SceneManager = container.Resolve<ISceneManager>();
        }

        public TControl CreateControl<TControl>(Action<TControl> configurer = null, BaseControl parent = null) where TControl : BaseControl
        {
            var control = _container.Resolve<TControl>();
            configurer?.Invoke(control);
            (parent?.Controls ?? Controls).Add(control);
            return control;
        }

        protected override bool CheckTextureValid()
        {
            return true;
        }

        protected override void DrawTexture() { }
    }
}
