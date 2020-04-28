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

        public Context<SpriteBatch> ActiveContext { get; private set; }

        public DrawerManager(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Device = device;
            _spriteBatch = spriteBatch;
        }

        public Context<SpriteBatch> PrepareSpriteBatch()
        {
            if (ActiveContext != null)
            {
                _spriteBatch.End();
            }

            _spriteBatch.Begin(sortMode: SpriteSortMode.Texture);

            ActiveContext = new Context<SpriteBatch>(_spriteBatch, DisposeContext, ActiveContext);

            return ActiveContext;
        }

        private void DisposeContext(Context<SpriteBatch> context)
        {
            _spriteBatch.End();
            ActiveContext = context.ParentContext;
        }
    }
}
