using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class DrawerManager : IDrawerManager
    {
        public GraphicsDevice Device { get; private set; }

        private readonly SpriteBatch _spriteBatch;

        public DrawerContext ActiveContext { get; private set; }

        public DrawerManager(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Device = device;
            _spriteBatch = spriteBatch;
        }

        public DrawerContext BuildContext()
        {
            if (ActiveContext != null)
            {
                _spriteBatch.End();
            }

            _spriteBatch.Begin(sortMode: SpriteSortMode.Texture);

            ActiveContext = new DrawerContext(_spriteBatch, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        private void DisposeContext(DrawerContext context)
        {
            _spriteBatch.End();
            ActiveContext = context.ParentContext;
        }
    }
}
