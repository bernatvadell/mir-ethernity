using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Controls;
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

        public BaseScene(ILifetimeScope container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
            _graphics = container.Resolve<GraphicsDevice>();
            _spriteBatch = container.Resolve<SpriteBatch>();
        }

        public TControl CreateControl<TControl>() where TControl : BaseControl
        {
            var control = _container.Resolve<TControl>();
            Controls.Add(control);
            return control;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
