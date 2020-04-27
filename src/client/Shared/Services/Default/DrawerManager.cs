using Microsoft.Xna.Framework.Graphics;
using Mir.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Services.Default
{
    public class DrawerManager : IDrawerManager
    {
        private readonly SpriteBatch _spriteBatch;

        public DrawerContext ActiveContext { get; private set; }

        public DrawerManager(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public DrawerContext BuildContext()
        {
            if (ActiveContext != null)
            {
                _spriteBatch.End();
            }

            _spriteBatch.Begin();

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
